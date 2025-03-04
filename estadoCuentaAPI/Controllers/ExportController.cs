using estadoCuentaAPI.Services;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using Microsoft.AspNetCore.Mvc;
using Table = iText.Layout.Element.Table;
using System.IO;
using ClosedXML.Excel;
using estadoCuentaAPI.Interfaces;


namespace estadoCuentaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExportController : ControllerBase
    {
        private readonly ITarjetaCreditoService _tarjetaCreditoService;

        public ExportController(ITarjetaCreditoService tarjetaCreditoService)
        {
            _tarjetaCreditoService = tarjetaCreditoService;
        }

        /// <summary>
        /// exporta el estado de cuenta en un archivo PDF
        /// </summary>
        /// <param name="tarjetaCreditoId">Id de tarjeta de credito a consultar</param>
        /// <returns>
        ///     El archivo en PDF del estado de cuenta
        ///     200 si se generó correctamente
        ///     500 si hubo un error
        /// </returns>
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("ExportarEstadoCuentaPDF/{tarjetaCreditoId}")]
        public async Task<IActionResult> ExportarEstadoCuentaPDF(int tarjetaCreditoId)
        {
            string filePath = "";
            try
            {
                var estadoCuenta = await _tarjetaCreditoService.ObtenerEstadoCuentaAsync(tarjetaCreditoId);
                if (estadoCuenta == null)
                {
                    return NotFound("No se encontró el estado de cuenta para esta tarjeta.");
                }

                var terminacionTarjeta = estadoCuenta.NumeroTarjeta[12..];
                filePath = $"Estado de cuenta {terminacionTarjeta}.pdf";

                // Crear PDF 
                using (MemoryStream ms = new MemoryStream())
                {
                    using (PdfWriter writer = new PdfWriter(ms))
                    {
                        using (PdfDocument pdfDocument = new PdfDocument(writer))
                        {
                            Document document = new(pdfDocument);

                            // Agregar título y otros campos generales
                            document.Add(new Paragraph("Estado de Cuenta de Tarjeta de Crédito"));

                            document.Add(new Paragraph($"Titular: {estadoCuenta.NombreTitular}"));
                            document.Add(new Paragraph($"Número de Tarjeta: {estadoCuenta.NumeroTarjeta}"));
                            document.Add(new Paragraph($"Saldo Utilizado: ${estadoCuenta.SaldoUtilizado}"));
                            document.Add(new Paragraph($"Saldo Disponible: ${estadoCuenta.SaldoDisponible}"));
                            document.Add(new Paragraph($"Límite de Crédito: ${estadoCuenta.LimiteCredito}"));
                            document.Add(new Paragraph(" "));

                            // Agregar tabla de movimientos
                            Table table = new Table(3, true);

                            // Agregar Encabezados de la tabla
                            table.AddHeaderCell("Fecha");
                            table.AddHeaderCell("Descripción");
                            table.AddHeaderCell("Monto");

                            foreach (var movimiento in estadoCuenta.Movimientos)
                            {
                                table.AddCell(movimiento.FechaMoviomiento.ToString("dd/MM/yyyy"));
                                table.AddCell(movimiento.Descripcion);
                                table.AddCell($"${movimiento.Monto}");
                            }

                            document.Add(table);
                            document.Close();

                        }
                    }
                    byte[] fileBytes = ms.ToArray();
                    return File(fileBytes, "application/pdf", $"Estado de cuenta {terminacionTarjeta}.pdf");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
            finally
            {
                // verificamos si el archivo existe para borrarlo despues de la descarga
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                
                }
                
            }
        }

        /// <summary>
        /// Excel de los movimiento de las compras realizadas por el cliente  en un rango de fechas especificado
        /// </summary>
        /// <param name="fechaFin">fecha de fin de la consulta</param>
        /// <param name="fechaInicio">Fecha de inicio de la consulta</param>
        /// <param name="tarjetaCreditoId">Id de tarjeta de credito a consultar</param>
        /// <returns>
        ///     Un archivo en excel o el contenido del mensaje de error
        ///     200 si se generó correctamente
        ///     400 si los parámetros no son válidos
        ///     404 si no se encontraron compras en el rango de fechas especificado
        ///     500 si hubo un error
        /// </returns>
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet("ExportarComprasExcel")]
        public async Task<IActionResult> ExportarComprasExcel(int tarjetaCreditoId, DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                if (tarjetaCreditoId <= 0)
                {
                    return BadRequest("El ID de la tarjeta de crédito no es válido.");
                }

                if (fechaInicio > fechaFin)
                {
                    return BadRequest("La fecha de inicio no puede ser mayor que la fecha de fin.");
                }

                var compras = await _tarjetaCreditoService.ObtenerComprasEnRangoAsync(tarjetaCreditoId, fechaInicio, fechaFin);

                if (compras == null || !compras.Any())
                {
                    return NotFound("No se encontraron compras en el rango de fechas especificado.");
                }

                using var workbook = new XLWorkbook();
                var worksheet = workbook.Worksheets.Add("Compras");

                // Crear encabezados
                worksheet.Cell(1, 1).Value = "Fecha Movimiento";
                worksheet.Cell(1, 2).Value = "Descripción";
                worksheet.Cell(1, 3).Value = "Monto";

                // Llenar datos
                int row = 2;
                foreach (var compra in compras)
                {
                    worksheet.Cell(row, 1).Value = compra.FechaMoviomiento.ToString("dd-MM-yyyy");
                    worksheet.Cell(row, 2).Value = compra.Descripcion;
                    worksheet.Cell(row, 3).Value = compra.Monto;
                    row++;
                }

                // Ajustar columnas
                worksheet.Columns().AdjustToContents();

                // Guardar en memoria
                using var stream = new MemoryStream();
                workbook.SaveAs(stream);
                stream.Position = 0;

                return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Compras.xlsx");
            }
            catch (ArgumentException argEx)
            {
                //  parámetros no válidos
                return BadRequest(argEx.Message);
            }
            catch (IOException ioEx)
            {
                // problemas con la creación o guardado del archivo
                return StatusCode(500, "Hubo un problema al generar el archivo Excel: " + ioEx.Message);
            }
            catch (Exception ex)
            {
                // Manejar cualquier otro error
                return StatusCode(500, "Hubo un error inesperado: " + ex.Message);
            }
        }

    }
}

document.addEventListener("DOMContentLoaded", function () {
    const backendUrl = window.appConfig?.backendUrl || "";

    // Obtener lista de tarjetas
    $.ajax({
        url: `${backendUrl}/TarjetaCredito/ObtenerListaTarjetas`,
        type: "GET",
        success: function (data) {
            let rows = "";
            data.forEach(tarjeta => {
                rows += `<tr>
                            <td>${tarjeta.nombreCompletoCliente}</td>
                            <td>${tarjeta.numeroTarjeta}</td>                            
                            <td>
                                <button class="btn btn-success btn-sm" data-tarjeta-id="${tarjeta.tarjetaCreditoId}">Estado</button>
                                <button class="btn btn-primary btn-sm" data-tarjeta-id="${tarjeta.tarjetaCreditoId}">Historial</button>
                                <button class="btn btn-warning btn-sm" data-tarjeta-id="${tarjeta.tarjetaCreditoId}">Excel Compra</button>
                                <button class="btn btn-danger btn-sm" data-tarjeta-id="${tarjeta.tarjetaCreditoId}">Registrar Compra</button>
                                <button class="btn btn-secondary btn-sm" data-tarjeta-id="${tarjeta.tarjetaCreditoId}">Registrar Pago</button>
                            </td>
                         </tr>`;
            });
            $("#tarjetasTable").html(rows);

            // Asignar los eventos a los botones dinámicamente
            document.querySelectorAll("[data-tarjeta-id]").forEach(button => {
                const tarjetaCreditoId = button.getAttribute("data-tarjeta-id");

                document.getElementById('Id').value = tarjetaCreditoId;

                // Asignar eventos a los botones
                if (button.textContent === "Estado") {
                    button.addEventListener("click", () => estadoCuenta(tarjetaCreditoId));
                }
                if (button.textContent === "Historial") {
                    button.addEventListener("click", () => historialtarjeta(tarjetaCreditoId));
                }
                if (button.textContent === "Excel Compra") {
                    button.addEventListener("click", () => excelCompra(tarjetaCreditoId));
                }
                if (button.textContent === "Registrar Compra") {
                    button.addEventListener("click", () => registrarCompra(tarjetaCreditoId));
                }
                if (button.textContent === "Registrar Pago") {
                    button.addEventListener("click", () => registrarPago(tarjetaCreditoId));
                }
            });
        },
        error: function () {
            Swal.fire("Error", "No se pudo obtener la lista de tarjetas.", "error");
        }
    });

    // Función para obtener estado de cuenta
    function estadoCuenta(tarjetaCreditoId) {
        fetch(`/EstadoCuenta/Index?tarjetaCreditoId=${tarjetaCreditoId}`, {
            method: 'GET',
            headers: {
                'Accept': 'application/json'
            }
        })
            .then(response => {
                if (!response.ok) {
                    throw new Error('Error al generar el Estado de cuenta');
                }
                
                return response.text();
            })
            .then(html => {
                // Asegúrate de que el contenido no esté vacío
                if (!html.trim()) {
                    throw new Error('El contenido HTML está vacío.');
                }

                document.body.innerHTML = html; // Actualizar el contenido del body con la vista

                // Llamar a obtenerEstadoData después de cargar la vista
                obtenerEstadoData(tarjetaCreditoId, backendUrl);
            })
            .catch(error => {
                Swal.fire('Error', error.message, 'error');
            });
    }

    // Función para obtener historial de la tarjeta
    function historialtarjeta(tarjetaCreditoId) {
        fetch(`/Historial/Index?tarjetaCreditoId=${tarjetaCreditoId}`, {
            method: 'GET',
            headers: {
                'Accept': 'application/json'
            }
        })
            .then(response => {
                if (!response.ok) {
                    throw new Error('Error al generar el historial');
                }
                return response.text(); 
            })
            .then(html => {
                document.body.innerHTML = html;
                obtenerHistorialData(tarjetaCreditoId, backendUrl);
            })
            .catch(error => {
                Swal.fire('Error', error.message, 'error');
            });
    }


    // Función para exportar compras a Excel
    function excelCompra(tarjetaCreditoId) {
        Swal.fire({
            title: 'Excel de Compras (por fecha)',
            html:
                '<input id="fechaInicio" type="date" class="swal2-input" placeholder="Fecha Inicio">' +
                '<input id="fechaFin" type="date" class="swal2-input" placeholder="Fecha Fin">',
            focusConfirm: false,
            showCancelButton: true,
            confirmButtonText: 'Generar Excel',
            cancelButtonText: 'Cancelar',
            preConfirm: () => {
                const fechaInicio = document.getElementById('fechaInicio').value.trim();
                const fechaFin = document.getElementById('fechaFin').value.trim();

                if (!fechaInicio || !fechaFin) {
                    Swal.showValidationMessage('Todos los campos son obligatorios');
                    return false;
                }

                return { fechaInicio, fechaFin, tarjetaCreditoId };
            }
        }).then((result) => {
            if (result.isConfirmed && result.value) {
                const { fechaInicio, fechaFin, tarjetaCreditoId } = result.value;

                if (!backendUrl) {
                    Swal.fire('Error', 'No se encontró la URL del backend', 'error');
                    return;
                }

                // Construimos la URL con los parámetros en la query string
                const url = `${backendUrl}/Export/ExportarComprasExcel?tarjetaCreditoId=${tarjetaCreditoId}&fechaInicio=${encodeURIComponent(fechaInicio)}&fechaFin=${encodeURIComponent(fechaFin)}`;

                // Realizamos la petición para descargar el archivo
                fetch(url, {
                    method: 'GET',
                    headers: {
                        'Accept': 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' // Tipo de archivo Excel
                    }
                })
                    .then(response => {
                        console.log(response);
                        if (!response.ok) {
                            throw new Error('Error al generar el archivo Excel');
                        }
                        return response.blob();
                    })
                    .then(blob => {
                        // Crear un enlace de descarga
                        const link = document.createElement('a');
                        link.href = URL.createObjectURL(blob);
                        link.download = `Compras_${fechaInicio}_a_${fechaFin}.xlsx`;
                        document.body.appendChild(link);
                        link.click();
                        document.body.removeChild(link);

                        Swal.fire('Archivo generado', 'Se ha descargado el archivo Excel correctamente.', 'success');
                    })
                    .catch(error => {
                        Swal.fire('Error', error.message, 'error');
                    });
            }
        });
    }


    // Función para registrar compra
    function registrarCompra(tarjetaCreditoId) {
        Swal.fire({
            title: 'Registro de la Compra',
            html:
                '<input id="fechaMovimiento" type="date" class="swal2-input" placeholder="Fecha del Movimiento">' +
                '<input id="descripcion" type="text" class="swal2-input" placeholder="Descripcion">' +
                '<input id="monto" type="number" class="swal2-input" placeholder="monto">',
            focusConfirm: false,
            showCancelButton: true,
            confirmButtonText: 'Compra',
            cancelButtonText: 'Cancelar',
            preConfirm: () => {
                const fechaMovimiento = document.getElementById('fechaMovimiento').value.trim();
                const descripcion = document.getElementById('descripcion').value.trim();
                const monto = parseFloat(document.getElementById('monto').value.trim());

                // Validar campos
                if (!descripcion || !monto || !fechaMovimiento) {
                    Swal.showValidationMessage('Todos los campos son obligatorios');
                    return false;
                }

                if (isNaN(monto) || monto <= 0) {
                    Swal.showValidationMessage('El monto debe ser un número positivo');
                    return false;
                }

                return { tarjetaCreditoId, fechaMovimiento, descripcion, monto };
            }
        }).then((result) => {
            if (result.isConfirmed && result.value) {
                const movimiento = result.value;

                if (!backendUrl) {
                    Swal.fire('Error', 'No se encontró la URL del backend', 'error');
                    return;
                }

                // Realizamos la petición para registrar la compra
                fetch(`${backendUrl}/Movimiento/RegistrarCompra`, {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                        'Accept': 'application/json'
                    },
                    body: JSON.stringify(movimiento)
                })
                    .then(response => {
                        if (!response.ok) {
                            throw new Error('Error al guardar la compra');
                        }
                        return response.json();
                    })
                    .then(data => {
                        Swal.fire('Compra Guardada', '', 'success');
                    })
                    .catch(error => {
                        Swal.fire('Error', error.message, 'error');
                    });
            }
        });
    }

    // Función para registrar pago
    function registrarPago(tarjetaCreditoId) {
        Swal.fire({
            title: 'Registro del Pago',
            html:
                '<input id="fechaMovimiento" type="date" class="swal2-input" placeholder="Fecha del Movimiento">' +
                '<input id="descripcion" type="text" class="swal2-input" placeholder="Descripcion">' +
                '<input id="monto" type="number" class="swal2-input" placeholder="Monto">',
            focusConfirm: false,
            showCancelButton: true,
            confirmButtonText: 'Pago',
            cancelButtonText: 'Cancelar',
            preConfirm: () => {
                const fechaMovimiento = document.getElementById('fechaMovimiento').value.trim();
                const descripcion = document.getElementById('descripcion').value.trim();
                const monto = parseFloat(document.getElementById('monto').value.trim());

                // Validar campos
                if (!descripcion || !monto || !fechaMovimiento) {
                    Swal.showValidationMessage('Todos los campos son obligatorios');
                    return false;
                }

                if (isNaN(monto) || monto <= 0) {
                    Swal.showValidationMessage('El monto debe ser un número positivo');
                    return false;
                }

                return { tarjetaCreditoId, fechaMovimiento, descripcion, monto };
            }
        }).then((result) => {
            if (result.isConfirmed && result.value) {
                const movimiento = result.value;

                if (!backendUrl) {
                    Swal.fire('Error', 'No se encontró la URL del backend', 'error');
                    return;
                }

                // Realizamos la petición para registrar el pago
                fetch(`${backendUrl}/Movimiento/RegistrarPago`, {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                        'Accept': 'application/json'
                    },
                    body: JSON.stringify(movimiento)
                })
                    .then(response => {
                        if (!response.ok) {
                            throw new Error('Error al guardar el pago');
                        }
                        return response.json();
                    })
                    .then(data => {
                        Swal.fire('Pago Guardado', '', 'success');
                    })
                    .catch(error => {
                        Swal.fire('Error', error.message, 'error');
                    });
            }
        });
    }
   
});


function obtenerHistorialData(id, backendUrl) {
    // Obtener historial de la tarjeta
    $.ajax({
        url: `${backendUrl}/HistorialTransacciones/historialTransacciones/${id}`,
        type: "GET",
        success: function (data) {

            let rows = "";
            data.forEach(historia => {
                // Convertir la fecha a un objeto Date
                let fecha = new Date(historia.fechaMoviomiento);

                // Formatear la fecha en el formato: dd/mm/yyyy
                let fechaFormateada = `${fecha.getDate().toString().padStart(2, '0')}/${(fecha.getMonth() + 1).toString().padStart(2, '0')}/${fecha.getFullYear()}`;

                rows += `<tr>
                        <td>${fechaFormateada}</td>
                        <td>${historia.descripcion}</td>                            
                        <td>${historia.monto}</td>
                        <td>${historia.tipoMovimiento}</td>
                     </tr>`;
            });
            $("#historialTable").html(rows);
        },
        error: function () {
            Swal.fire("Error", "No se pudo obtener el historial de la tarjeta.", "error");
        }
    });
}
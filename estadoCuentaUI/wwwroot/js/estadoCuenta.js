

function obtenerEstadoData(tarjetaCreditoId, backendUrl) {
    console.log("tarjetaCreditoId : ", tarjetaCreditoId)
    $.ajax({
        url: `${backendUrl}/EstadoCuenta/ObtenerEstadoCuenta/${tarjetaCreditoId}`,
        type: "GET",
        dataType: "json",
        success: function (data) {
            console.log("Respuesta de la API:", data);
            let rows = "";

            let fechaCorte = new Date(data.fechaCorte);
            let fechaCorteFormateada = `${fechaCorte.getDate().toString().padStart(2, '0')}/${(fechaCorte.getMonth() + 1).toString().padStart(2, '0')}/${fechaCorte.getFullYear()}`;

            // Asignar los valores a los inputs correspondientes
            document.getElementById("nombreTitular").value = data.nombreTitular;
            document.getElementById("tarjeta").value = data.numeroTarjeta;
            document.getElementById("disponible").value = data.saldoDisponible;
            document.getElementById("utilizado").value = data.saldoUtilizado;
            document.getElementById("limiteCredito").value = data.limiteCredito;
            document.getElementById("fechaCorte").value = fechaCorteFormateada;

            document.getElementById("montoTotalComprasMesActual").value = data.montoTotalComprasMesActual;
            document.getElementById("interesBonificable").value = data.interesBonificable;
            document.getElementById("montoTotalPagar").value = data.montoTotalPagar;
            document.getElementById("montoTotalComprasMesAnterior").value = data.montoTotalComprasMesAnterior;
            document.getElementById("cuotaMinima").value = data.cuotaMinima;
            document.getElementById("montoContadoConIntereses").value = data.montoContadoConIntereses;

            data.movimientos.forEach(cuenta => {
                // Convertir la fecha a un objeto Date
                let fecha = new Date(cuenta.fechaMoviomiento);

                // Formatear la fecha en el formato dd/mm/yyyy
                let fechaFormateada = `${fecha.getDate().toString().padStart(2, '0')}/${(fecha.getMonth() + 1).toString().padStart(2, '0')}/${fecha.getFullYear()}`;

                rows += `<tr>
                        <td>${fechaFormateada}</td>
                        <td>${cuenta.descripcion}</td>                            
                        <td>${cuenta.monto}</td>
                        <td>${cuenta.tipoMovimiento}</td>
                     </tr>`;
            });
            $("#estadoCuentaTable").html(rows);
        },
        error: function (xhr, status, error) {
            console.error("Error en AJAX:", status, error);
        }
    });
}

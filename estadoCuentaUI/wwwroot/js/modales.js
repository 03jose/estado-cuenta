// Obtener referencia al modal
const modal = document.getElementById("modalTarjeta");

if (modal) {
    modal.addEventListener("click", function (event) {
        if (event.target === modal) {
            modal.classList.remove("show");
            modal.style.display = "none";
            document.body.classList.remove("modal-open");
        }
    });
} else {
    console.error("El modal 'modalTarjeta' no se encontró en el DOM.");
}

// Mostrar el modal cuando se haga clic en el botón 'Nueva tarjeta'
const btnAbrirModal = document.getElementById("agregarTarjetaModal");

if (btnAbrirModal) {
    btnAbrirModal.addEventListener("click", function () {
        if (modal) {
            const modalInstance = new bootstrap.Modal(modal);
            modalInstance.show();
        }
    });
} else {
    console.error("El botón 'agregarTarjetaModal' no se encontró en el DOM.");
}

// Agregar el evento al botón de guardar dentro del modal
if (modal) {
    modal.addEventListener("shown.bs.modal", function () {
        const btnGuardar = document.getElementById("GuardarInfoClienteTarjeta");

        if (btnGuardar) {
            btnGuardar.addEventListener("click", function () {
                // Recolectar los datos del formulario
                const numeroTarjeta = document.getElementById("numeroTarjeta").value.trim();
                const limiteCredito = document.getElementById("limiteCredito").value.trim();
                const tasaInteres = document.getElementById("tasaInteres").value.trim();
                const fechaCorte = document.getElementById("fechaCorte").value.trim();
                const fechaPago = document.getElementById("fechaPago").value.trim();

                // Validación de campos
                if (!numeroTarjeta || !limiteCredito || !tasaInteres || !fechaCorte || !fechaPago) {
                    Swal.showValidationMessage("Todos los campos son obligatorios");
                    return false;
                }

                const tarjeta = {
                    numeroTarjeta: numeroTarjeta,
                    limiteCredito: limiteCredito,
                    tasaInteres: tasaInteres,
                    fechaCorte: fechaCorte,
                    fechaPago: fechaPago
                };

                // Enviar la solicitud de creación de tarjeta
                fetch(`${backendUrl}/TarjetaCredito/AgregarTarjtetaCliente`, {
                    method: "POST",
                    headers: {
                        "Content-Type": "application/json",
                        "Accept": "application/json"
                    },
                    body: JSON.stringify(tarjeta)
                })
                    .then(response => {
                        if (!response.ok) {
                            throw new Error("Error al guardar tarjeta");
                        }
                        return response.json();
                    })
                    .then(data => {
                        Swal.fire("Tarjeta Guardada", "", "success");
                        // Cerrar el modal después de guardar
                        if (modal) {
                            const modalInstance = bootstrap.Modal.getInstance(modal);
                            modalInstance.hide();
                        }
                    })
                    .catch(error => {
                        Swal.fire("Error", error.message, "error");
                    });
            });
        } else {
            console.error("El botón 'GuardarInfoClienteTarjeta' no se encontró en el DOM.");
        }
    });
}
document.addEventListener("DOMContentLoaded", function () {
    document.getElementById("abrirModalBtn").addEventListener("click", function () {
        fetch("tarjetas/modal/AgregarTarjetaCliente.html") // Ruta donde está el modal
            .then(response => response.text())
            .then(html => {
                document.getElementById("modalContainer").innerHTML = html; // Inserta el modal en la página
                let modal = new bootstrap.Modal(document.getElementById("modalTarjeta")); // Inicializa el modal
                modal.show(); // Muestra el modal
            })
            .catch(error => console.error("Error cargando el modal:", error));
    });
});
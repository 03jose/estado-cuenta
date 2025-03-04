document.addEventListener("DOMContentLoaded", function () {
    const backendUrl = window.appConfig?.backendUrl || "";

    document.getElementById('agregarClienteModal').addEventListener('click', function () {
        
        Swal.fire({
            title: 'Nuevo Cliente',
            html:
                '<input id="nombre" class="swal2-input" maxlength="50" placeholder="Nombre">' +
                '<input id="apellido" class="swal2-input" maxlength="50" placeholder="Apellido">' +
                '<input id="dui" class="swal2-input" maxlength="10" placeholder="DUI">',
            focusConfirm: false,
            showCancelButton: true,
            confirmButtonText: 'Guardar',
            cancelButtonText: 'Cancelar',
            preConfirm: () => {
                const nombre = document.getElementById('nombre').value.trim();
                const apellido = document.getElementById('apellido').value.trim();
                const dui = document.getElementById('dui').value.trim();

                if (!nombre || !apellido || !dui) {
                    Swal.showValidationMessage('Todos los campos son obligatorios');
                    return false;
                }

                return { nombre, apellido, dui };
            }
        }).then((result) => {
            if (result.isConfirmed && result.value) {
                const cliente = result.value;

                if (!backendUrl) {
                    Swal.fire('Error', 'No se encontró la URL del backend', 'error');
                    return;
                }

                fetch(`${backendUrl}/Cliente/crear`, {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                        'Accept': 'application/json'
                    },
                    body: JSON.stringify(cliente)
                })
                    .then(response => {
                        if (!response.ok) {
                            throw new Error('Error al guardar cliente');
                        }
                        return response.json();
                    })
                    .then(data => {
                        Swal.fire('Cliente Guardado', '', 'success');
                    })
                    .catch(error => {
                        Swal.fire('Error', error.message, 'error');
                    });
            }
        });
    });
});

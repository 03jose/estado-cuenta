function obtenerPDF() {
    const backendUrl = window.appConfig?.backendUrl;
    const id = document.getElementById('Id')?.value;

    if (!backendUrl || !id) {
        console.error("Error: backendUrl o ID no definidos.");
        return;
    }

    fetch(`${backendUrl}/Export/ExportarEstadoCuentaPDF/${id}`, {
        method: 'GET',
        headers: { 'Content-Type': 'application/json' },
    })
        .then(response => {
            if (!response.ok) {
                throw new Error('Error al obtener el PDF');
            }
            return response.blob();
        })
        .then(blob => {
            const url = window.URL.createObjectURL(blob);
            const a = document.createElement('a');
            a.style.display = 'none';
            a.href = url;
            a.download = 'estado_de_cuenta.pdf';
            document.body.appendChild(a);
            a.click();
            window.URL.revokeObjectURL(url);
        })
        .catch(error => {
            console.error('Error al descargar el PDF:', error);
        });
}


# estado-cuenta
La aplicación debe permitir visualizar el estado de cuenta de una tarjeta de  crédito, incluyendo el detalle de movimientos, cálculo de cuota mínima, cálculo de contado e interés  bonificable, y mostrar el saldo utilizado y disponible de la tarjeta de crédito. 

Carpetas principales:
1-  Documentacion que contiene los scripts de la base, el documento de word explicando la arquitectura y el postman collection con la prueba 
2-  La API o backend en un proyecto
3-  La UI o frontend en un proyecto ambos en la misma solucion


#	Elaborado por: Jose Ricardo Martinez Ruano

##  Instalación ### 
Requisitos previos Antes de comenzar, asegúrate de tener instalado lo siguiente: 
  - [.NET SDK](https://dotnet.microsoft.com/download) 
  - [SQL Server](https://www.microsoft.com/es-es/sql-server/sql-server-downloads) 
  - [Postman (Opcional)](https://www.postman.com/downloads/) para probar la API. 

### Clonar el repositorio ###
	sh git clone https://github.com/03jose/estado-cuenta.git cd estado-cuenta

###  Configurar la base de datos ###
	1- Crea una base de datos en SQL Server (CuentasDB).
	2- Ejecuta los scripts SQL ubicados en la carpeta /Documentacion/database
		a. Estructura de la base:  base de datos.sql
		b. Datos de prueba: datos de prueba.sql
		c. Procedimientos Alamacenados: procedimientos almacenados.sql
	3- Configura la cadena de conexión en appsettings.json en el proyecto estadoCuentaAPI:
		"ConnectionStrings": {
			"DefaultConnection": "Server=tu_servidor;Database= CuentasDB;User Id=usuario;Password=contraseña;"
		}
	4- Agrega en el appsetting.json del proyecto estadoCuentaAPI la dirección del frontend (por ejemplo):
	"frontEndUrl": "https://localhost:7007",
	5- Agrega en el appsettings.json del proyecto estadoCuentaUI la dirección al Backend (por ejemplo):
	"BackendUrl": "https://localhost:7049/api", 

### Tecnologías utilizadas ###
	- .NET Core 6
	- Entity Framework Core
	- SQL Server
	- JavaScript (Fetch API, jQuery AJAX)
	- iText7
	- FluentValidation
	- Automapper
	- ClosedXML
	- Swagger
	- Healthcheck

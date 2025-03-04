---------------- ObtenerMovimientosTarjeta ------------------------
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Jose Ricardo Martinez Ruano
-- Create date: 27/02/2025
-- Description:	Obtencion de los movimientos de la tarjeta
-- =============================================
CREATE PROCEDURE ObtenerMovimientosTarjeta
	 @TarjetaCreditoId INT,
    @FechaInicio DATE,
    @FechaFin DATE
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		Movimiento_Tarjeta_ID,
		TarjetaCredito_ID,
		 Fecha_Movimiento,
		 CONVERT(VARCHAR, Fecha_Movimiento, 120) AS FechaMovimiento,
		Descripcion,
		Monto,
		Tipo_Movimiento AS TipoMovimiento,
		Tipo_Movimiento
	FROM MovimientoTarjeta
	WHERE TarjetaCredito_ID = @TarjetaCreditoId AND
		Fecha_Movimiento BETWEEN @FechaInicio  AND @FechaFin
		AND Tipo_Movimiento LIKE '%compra%'
	ORDER BY FechaMovimiento DESC;
END
GO

------------------ ObtenerMovimientosTarjetaPorId 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Jose Ricardo Martinez Ruano
-- Create date: 27/02/2025
-- Description:	Obtencion de los movimientos de la tarjeta
-- =============================================
CREATE PROCEDURE ObtenerMovimientosTarjetaPorId
	 @TarjetaCreditoId INT    
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		Movimiento_Tarjeta_ID,
		Fecha_Movimiento,
		Fecha_Movimiento AS FechaMovimiento,
		Descripcion,
		Monto,
		Tipo_Movimiento AS TipoMovimiento,
		Tipo_Movimiento,
		TarjetaCredito_ID
	FROM MovimientoTarjeta
	WHERE TarjetaCredito_ID = @TarjetaCreditoId 
    AND (
        -- Mes actual en el año actual
        (MONTH(Fecha_Movimiento) = MONTH(GETDATE()) AND YEAR(Fecha_Movimiento) = YEAR(GETDATE()))
        -- Mes anterior en el año correspondiente
        OR (MONTH(Fecha_Movimiento) = MONTH(DATEADD(MONTH, -1, GETDATE())) 
            AND YEAR(Fecha_Movimiento) = YEAR(DATEADD(MONTH, -1, GETDATE())))
    )
	ORDER BY FechaMovimiento DESC;
END
GO

-------------------- obtener cliente 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Jose Ricardo Martinez Ruano
-- Create date: 27/02/2025
-- Description:	Obtencion de informacion del cliente
-- =============================================
CREATE PROCEDURE ObtenerClientePorId
	 @clienteId INT    
AS
BEGIN
	SET NOCOUNT ON;

	SELECT TOP 1 *
	FROM Cliente
	WHERE Cliente_ID = @clienteId 
    
END
GO

-------------------Obtener informacion de la tarjeta
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Jose Ricardo Martinez Ruano
-- Create date: 27/02/2025
-- Description:	Obtencion de informacion del cliente
-- =============================================
CREATE PROCEDURE ObtenerInformacionTarjeta
	 @TarjetaCreditoId INT    
AS
BEGIN
	SET NOCOUNT ON;

	SELECT TOP 1 *
	FROM TarjetaCredito
	WHERE TarjetaCredito_ID = @TarjetaCreditoId 
    
END

---------------------------ObtenerListadoTarjetas
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Jose Ricardo Martinez Ruano
-- Create date: 28/02/2025
-- Description:	Obtencion de las tarjetas
-- =============================================
CREATE PROCEDURE ObtenerListadoTarjetas
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT  
		tc.Cliente_ID AS ClienteId,
		tc.TarjetaCredito_ID AS TarjetaCreditoId,
		tc.Numero_Tarjeta AS NumeroTarjeta,
		c.Nombre + ' ' + c.Apellido AS NombreCompletoCliente
	FROM TarjetaCredito tc
		JOIN Cliente c on  c.Cliente_ID =  tc.Cliente_ID
	
END
GO
--------------------------------------------

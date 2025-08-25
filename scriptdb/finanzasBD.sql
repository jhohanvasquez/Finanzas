USE [FinanzasDB]
GO
/****** Object:  Table [dbo].[Deuda]    Script Date: 25/08/2025 7:21:34 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Deuda](
	[DeudaId] [int] IDENTITY(1,1) NOT NULL,
	[UsuarioId] [int] NOT NULL,
	[MontoTotal] [decimal](18, 2) NOT NULL,
	[Descripcion] [nvarchar](200) NULL,
	[FechaCreacion] [datetime] NULL,
	[Estado] [varchar](20) NULL,
PRIMARY KEY CLUSTERED 
(
	[DeudaId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Pago]    Script Date: 25/08/2025 7:21:34 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Pago](
	[PagoId] [int] IDENTITY(1,1) NOT NULL,
	[DeudaId] [int] NOT NULL,
	[MontoPago] [decimal](18, 2) NOT NULL,
	[FechaPago] [datetime] NULL,
	[MetodoPago] [varchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[PagoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Usuario]    Script Date: 25/08/2025 7:21:34 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Usuario](
	[UsuarioId] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](100) NOT NULL,
	[Email] [nvarchar](100) NOT NULL,
	[PasswordHash] [nvarchar](200) NOT NULL,
	[FechaRegistro] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[UsuarioId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Deuda] ADD  DEFAULT (getdate()) FOR [FechaCreacion]
GO
ALTER TABLE [dbo].[Deuda] ADD  DEFAULT ('Pendiente') FOR [Estado]
GO
ALTER TABLE [dbo].[Pago] ADD  DEFAULT (getdate()) FOR [FechaPago]
GO
ALTER TABLE [dbo].[Usuario] ADD  DEFAULT (getdate()) FOR [FechaRegistro]
GO
ALTER TABLE [dbo].[Deuda]  WITH CHECK ADD FOREIGN KEY([UsuarioId])
REFERENCES [dbo].[Usuario] ([UsuarioId])
GO
ALTER TABLE [dbo].[Pago]  WITH CHECK ADD FOREIGN KEY([DeudaId])
REFERENCES [dbo].[Deuda] ([DeudaId])
GO
/****** Object:  StoredProcedure [dbo].[sp_ConsultarDeudas]    Script Date: 25/08/2025 7:21:34 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[sp_ConsultarDeudas]
    @UsuarioId INT = NULL
AS
BEGIN
    SELECT 
        u.Nombre,
        d.DeudaId,
        d.MontoTotal,
        ISNULL(SUM(pg.MontoPago), 0) AS TotalPagado,
        (d.MontoTotal - ISNULL(SUM(pg.MontoPago), 0)) AS SaldoPendiente,
        d.Estado
    FROM Deuda d
    INNER JOIN Usuario u ON d.UsuarioId = u.UsuarioId
    LEFT JOIN Pago pg ON d.DeudaId = pg.DeudaId
    WHERE (@UsuarioId IS NULL OR d.UsuarioId = @UsuarioId)
    GROUP BY u.Nombre, d.DeudaId, d.MontoTotal, d.Estado;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_RegistrarDeuda]    Script Date: 25/08/2025 7:21:34 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_RegistrarDeuda]
    @UsuarioId INT,
    @MontoTotal DECIMAL(18,2),
    @Descripcion NVARCHAR(255) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF (@MontoTotal <= 0)
    BEGIN
        RAISERROR('El monto de la deuda debe ser mayor que cero.', 16, 1);
        RETURN;
    END;

    INSERT INTO Deuda (UsuarioId, MontoTotal, Descripcion, Estado)
    VALUES (@UsuarioId, @MontoTotal, @Descripcion, 'Pendiente');
END;
GO
/****** Object:  StoredProcedure [dbo].[sp_RegistrarPago]    Script Date: 25/08/2025 7:21:34 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[sp_RegistrarPago]
    @DeudaId INT,
    @MontoPago DECIMAL(18,2),
    @MetodoPago VARCHAR(50) = NULL
AS
BEGIN
    BEGIN TRANSACTION;

    BEGIN TRY
        
        INSERT INTO Pago (DeudaId, MontoPago, MetodoPago)
        VALUES (@DeudaId, @MontoPago, @MetodoPago);

       
        DECLARE @MontoTotal DECIMAL(18,2),
                @TotalPagado DECIMAL(18,2);

        SELECT @MontoTotal = MontoTotal FROM Deuda WHERE DeudaId = @DeudaId;

        SELECT @TotalPagado = ISNULL(SUM(MontoPago),0)
        FROM Pago WHERE DeudaId = @DeudaId;

        UPDATE Deuda
        SET Estado = CASE
                        WHEN @TotalPagado = 0 THEN 'Pendiente'
                        WHEN @TotalPagado < @MontoTotal THEN 'Parcial'
                        ELSE 'Pagada'
                     END
        WHERE DeudaId = @DeudaId;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        RAISERROR('Error al registrar el pago', 16, 1);
    END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[sp_RegistrarUsuario]    Script Date: 25/08/2025 7:21:34 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_RegistrarUsuario]
    @Nombre NVARCHAR(100),
    @Email NVARCHAR(100),
    @PasswordHash NVARCHAR(256)
AS
BEGIN
    SET NOCOUNT ON;

    -- Verificar si el usuario ya existe por Email
    IF EXISTS (SELECT 1 FROM Usuario WHERE Email = @Email)
    BEGIN
        -- Opcional: puedes devolver un código específico, por ejemplo -1
        RETURN -1
    END

    -- Insertar el nuevo usuario
    INSERT INTO Usuario (Nombre, Email, PasswordHash)
    VALUES (@Nombre, @Email, @PasswordHash)

    -- Opcional: devolver el número de filas afectadas
    RETURN @@ROWCOUNT
END

GO

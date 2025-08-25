USE [FinanzasDB]
GO
/****** Object:  Table [dbo].[Deuda]    Script Date: 24/08/2025 10:15:29 p. m. ******/
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
/****** Object:  Table [dbo].[Pago]    Script Date: 24/08/2025 10:15:29 p. m. ******/
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
/****** Object:  Table [dbo].[Usuario]    Script Date: 24/08/2025 10:15:29 p. m. ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Deuda] ON 
GO
INSERT [dbo].[Deuda] ([DeudaId], [UsuarioId], [MontoTotal], [Descripcion], [FechaCreacion], [Estado]) VALUES (1, 1, CAST(500000.00 AS Decimal(18, 2)), N'Préstamo personal', CAST(N'2025-08-23T00:50:58.557' AS DateTime), N'Parcial')
GO
INSERT [dbo].[Deuda] ([DeudaId], [UsuarioId], [MontoTotal], [Descripcion], [FechaCreacion], [Estado]) VALUES (2, 2, CAST(300000.00 AS Decimal(18, 2)), N'Compra de electrodoméstico', CAST(N'2025-08-23T00:50:58.557' AS DateTime), N'Pagada')
GO
SET IDENTITY_INSERT [dbo].[Deuda] OFF
GO
SET IDENTITY_INSERT [dbo].[Pago] ON 
GO
INSERT [dbo].[Pago] ([PagoId], [DeudaId], [MontoPago], [FechaPago], [MetodoPago]) VALUES (1, 1, CAST(200000.00 AS Decimal(18, 2)), CAST(N'2025-08-23T00:50:58.563' AS DateTime), N'Transferencia')
GO
INSERT [dbo].[Pago] ([PagoId], [DeudaId], [MontoPago], [FechaPago], [MetodoPago]) VALUES (2, 2, CAST(300000.00 AS Decimal(18, 2)), CAST(N'2025-08-23T00:50:58.563' AS DateTime), N'Efectivo')
GO
SET IDENTITY_INSERT [dbo].[Pago] OFF
GO
SET IDENTITY_INSERT [dbo].[Usuario] ON 
GO
INSERT [dbo].[Usuario] ([UsuarioId], [Nombre], [Email], [PasswordHash], [FechaRegistro]) VALUES (1, N'Carlos Pérez', N'carlos@email.com', N'hash123', CAST(N'2025-08-23T00:50:58.553' AS DateTime))
GO
INSERT [dbo].[Usuario] ([UsuarioId], [Nombre], [Email], [PasswordHash], [FechaRegistro]) VALUES (2, N'Ana Gómez', N'ana@email.com', N'hash456', CAST(N'2025-08-23T00:50:58.553' AS DateTime))
GO
INSERT [dbo].[Usuario] ([UsuarioId], [Nombre], [Email], [PasswordHash], [FechaRegistro]) VALUES (3, N'juan', N'juan@hotmail.com', N'321321', CAST(N'2025-08-23T17:21:54.257' AS DateTime))
GO
SET IDENTITY_INSERT [dbo].[Usuario] OFF
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Usuario__A9D10534023D5A04]    Script Date: 24/08/2025 10:15:29 p. m. ******/
ALTER TABLE [dbo].[Usuario] ADD UNIQUE NONCLUSTERED 
(
	[Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
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
/****** Object:  StoredProcedure [dbo].[sp_ConsultarDeudas]    Script Date: 24/08/2025 10:15:29 p. m. ******/
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
/****** Object:  StoredProcedure [dbo].[sp_RegistrarDeuda]    Script Date: 24/08/2025 10:15:29 p. m. ******/
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
/****** Object:  StoredProcedure [dbo].[sp_RegistrarPago]    Script Date: 24/08/2025 10:15:29 p. m. ******/
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
/****** Object:  StoredProcedure [dbo].[sp_RegistrarUsuario]    Script Date: 24/08/2025 10:15:29 p. m. ******/
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
    INSERT INTO Usuario (Nombre, Email, PasswordHash)
    VALUES (@Nombre, @Email, @PasswordHash);

    RETURN @@ROWCOUNT;
END

GO

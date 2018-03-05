
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 02/03/2018 10:52:06
-- Generated from EDMX file: C:\Users\Polaris\Documents\Proyecto Integrador\ProyectoIntegrador\Models\Model1.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [EgelTraining];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------


-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Alumno'
CREATE TABLE [dbo].[Alumno] (
    [Matricula] char(9)  NOT NULL,
    [Nombre] varchar(50)  NOT NULL,
    [ApellidoPaterno] varchar(50)  NOT NULL,
    [ApellidoMaterno] varchar(50)  NULL,
    [Carrera] varchar(4)  NOT NULL,
    [CorreoElectronico] varchar(256)  NULL,
    [PasswordHash] binary(64)  NULL
);
GO

-- Creating table 'Carrera'
CREATE TABLE [dbo].[Carrera] (
    [Siglas] varchar(4)  NOT NULL,
    [NombreLargo] varchar(100)  NOT NULL
);
GO

-- Creating table 'Examen'
CREATE TABLE [dbo].[Examen] (
    [IDExamen] int IDENTITY(1,1) NOT NULL,
    [Nombre] varchar(100)  NOT NULL,
    [Descripcion] varchar(max)  NULL,
    [FechaDeCreacion] datetime  NOT NULL,
    [Id] int  NULL
);
GO

-- Creating table 'PlanEstudios'
CREATE TABLE [dbo].[PlanEstudios] (
    [Siglas] varchar(4)  NOT NULL,
    [Plan] char(2)  NOT NULL
);
GO

-- Creating table 'Pregunta'
CREATE TABLE [dbo].[Pregunta] (
    [IDPregunta] int IDENTITY(1,1) NOT NULL,
    [Texto] varchar(max)  NOT NULL,
    [Imagen] varbinary(max)  NULL,
    [Tipo] smallint  NOT NULL,
    [TextoRespuesta] varchar(max)  NOT NULL,
    [RespuestaCorrecta] char(1)  NOT NULL,
    [Dificultad] smallint  NOT NULL,
    [Id] int  NULL
);
GO

-- Creating table 'QuestionInExam'
CREATE TABLE [dbo].[QuestionInExam] (
    [IDExamen] int  NOT NULL,
    [IDPregunta] int  NOT NULL,
    [Respuesta] char(1)  NULL,
    [Matricula] char(9)  NOT NULL
);
GO

-- Creating table 'RespuestasErroneas'
CREATE TABLE [dbo].[RespuestasErroneas] (
    [Consecutivo] int IDENTITY(1,1) NOT NULL,
    [ID_Pregunta] int  NOT NULL,
    [Opcion] char(1)  NOT NULL,
    [TextoRespuesta] varchar(max)  NOT NULL,
    [Explicacion] varchar(max)  NULL
);
GO

-- Creating table 'sysdiagrams'
CREATE TABLE [dbo].[sysdiagrams] (
    [name] nvarchar(128)  NOT NULL,
    [principal_id] int  NOT NULL,
    [diagram_id] int IDENTITY(1,1) NOT NULL,
    [version] int  NULL,
    [definition] varbinary(max)  NULL
);
GO

-- Creating table 'Tema'
CREATE TABLE [dbo].[Tema] (
    [ClaveTema] varchar(10)  NOT NULL,
    [NombreTema] varchar(70)  NOT NULL
);
GO

-- Creating table 'Usuario'
CREATE TABLE [dbo].[Usuario] (
    [Id] int  NOT NULL,
    [Nombre] nvarchar(50)  NOT NULL,
    [ApellidoPaterno] nvarchar(50)  NOT NULL,
    [ApellidoMaterno] nvarchar(50)  NULL,
    [CorreoElectronico] nvarchar(256)  NULL,
    [PasswordHash] binary(64)  NULL
);
GO

-- Creating table 'CarreraTieneTema'
CREATE TABLE [dbo].[CarreraTieneTema] (
    [Carrera_Siglas] varchar(4)  NOT NULL,
    [Tema_ClaveTema] varchar(10)  NOT NULL
);
GO

-- Creating table 'TemaEstaEnExamen'
CREATE TABLE [dbo].[TemaEstaEnExamen] (
    [Examen_IDExamen] int  NOT NULL,
    [Tema_ClaveTema] varchar(10)  NOT NULL
);
GO

-- Creating table 'TemaTienePregunta'
CREATE TABLE [dbo].[TemaTienePregunta] (
    [Pregunta_IDPregunta] int  NOT NULL,
    [Tema_ClaveTema] varchar(10)  NOT NULL
);
GO

-- Creating table 'AlumnoExamen'
CREATE TABLE [dbo].[AlumnoExamen] (
    [Alumno_Matricula] char(9)  NOT NULL,
    [Examen_IDExamen] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Matricula] in table 'Alumno'
ALTER TABLE [dbo].[Alumno]
ADD CONSTRAINT [PK_Alumno]
    PRIMARY KEY CLUSTERED ([Matricula] ASC);
GO

-- Creating primary key on [Siglas] in table 'Carrera'
ALTER TABLE [dbo].[Carrera]
ADD CONSTRAINT [PK_Carrera]
    PRIMARY KEY CLUSTERED ([Siglas] ASC);
GO

-- Creating primary key on [IDExamen] in table 'Examen'
ALTER TABLE [dbo].[Examen]
ADD CONSTRAINT [PK_Examen]
    PRIMARY KEY CLUSTERED ([IDExamen] ASC);
GO

-- Creating primary key on [Siglas], [Plan] in table 'PlanEstudios'
ALTER TABLE [dbo].[PlanEstudios]
ADD CONSTRAINT [PK_PlanEstudios]
    PRIMARY KEY CLUSTERED ([Siglas], [Plan] ASC);
GO

-- Creating primary key on [IDPregunta] in table 'Pregunta'
ALTER TABLE [dbo].[Pregunta]
ADD CONSTRAINT [PK_Pregunta]
    PRIMARY KEY CLUSTERED ([IDPregunta] ASC);
GO

-- Creating primary key on [IDExamen], [IDPregunta], [Matricula] in table 'QuestionInExam'
ALTER TABLE [dbo].[QuestionInExam]
ADD CONSTRAINT [PK_QuestionInExam]
    PRIMARY KEY CLUSTERED ([IDExamen], [IDPregunta], [Matricula] ASC);
GO

-- Creating primary key on [Consecutivo] in table 'RespuestasErroneas'
ALTER TABLE [dbo].[RespuestasErroneas]
ADD CONSTRAINT [PK_RespuestasErroneas]
    PRIMARY KEY CLUSTERED ([Consecutivo] ASC);
GO

-- Creating primary key on [diagram_id] in table 'sysdiagrams'
ALTER TABLE [dbo].[sysdiagrams]
ADD CONSTRAINT [PK_sysdiagrams]
    PRIMARY KEY CLUSTERED ([diagram_id] ASC);
GO

-- Creating primary key on [ClaveTema] in table 'Tema'
ALTER TABLE [dbo].[Tema]
ADD CONSTRAINT [PK_Tema]
    PRIMARY KEY CLUSTERED ([ClaveTema] ASC);
GO

-- Creating primary key on [Id] in table 'Usuario'
ALTER TABLE [dbo].[Usuario]
ADD CONSTRAINT [PK_Usuario]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Carrera_Siglas], [Tema_ClaveTema] in table 'CarreraTieneTema'
ALTER TABLE [dbo].[CarreraTieneTema]
ADD CONSTRAINT [PK_CarreraTieneTema]
    PRIMARY KEY CLUSTERED ([Carrera_Siglas], [Tema_ClaveTema] ASC);
GO

-- Creating primary key on [Examen_IDExamen], [Tema_ClaveTema] in table 'TemaEstaEnExamen'
ALTER TABLE [dbo].[TemaEstaEnExamen]
ADD CONSTRAINT [PK_TemaEstaEnExamen]
    PRIMARY KEY CLUSTERED ([Examen_IDExamen], [Tema_ClaveTema] ASC);
GO

-- Creating primary key on [Pregunta_IDPregunta], [Tema_ClaveTema] in table 'TemaTienePregunta'
ALTER TABLE [dbo].[TemaTienePregunta]
ADD CONSTRAINT [PK_TemaTienePregunta]
    PRIMARY KEY CLUSTERED ([Pregunta_IDPregunta], [Tema_ClaveTema] ASC);
GO

-- Creating primary key on [Alumno_Matricula], [Examen_IDExamen] in table 'AlumnoExamen'
ALTER TABLE [dbo].[AlumnoExamen]
ADD CONSTRAINT [PK_AlumnoExamen]
    PRIMARY KEY CLUSTERED ([Alumno_Matricula], [Examen_IDExamen] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [Carrera] in table 'Alumno'
ALTER TABLE [dbo].[Alumno]
ADD CONSTRAINT [FK_Alumno_Carrera]
    FOREIGN KEY ([Carrera])
    REFERENCES [dbo].[Carrera]
        ([Siglas])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Alumno_Carrera'
CREATE INDEX [IX_FK_Alumno_Carrera]
ON [dbo].[Alumno]
    ([Carrera]);
GO

-- Creating foreign key on [Matricula] in table 'QuestionInExam'
ALTER TABLE [dbo].[QuestionInExam]
ADD CONSTRAINT [FK_QuestionInExam_Alumno]
    FOREIGN KEY ([Matricula])
    REFERENCES [dbo].[Alumno]
        ([Matricula])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_QuestionInExam_Alumno'
CREATE INDEX [IX_FK_QuestionInExam_Alumno]
ON [dbo].[QuestionInExam]
    ([Matricula]);
GO

-- Creating foreign key on [Siglas] in table 'PlanEstudios'
ALTER TABLE [dbo].[PlanEstudios]
ADD CONSTRAINT [FK_PlanEstudios_Carrera]
    FOREIGN KEY ([Siglas])
    REFERENCES [dbo].[Carrera]
        ([Siglas])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Id] in table 'Examen'
ALTER TABLE [dbo].[Examen]
ADD CONSTRAINT [FK_Examen_Usuario]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[Usuario]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Examen_Usuario'
CREATE INDEX [IX_FK_Examen_Usuario]
ON [dbo].[Examen]
    ([Id]);
GO

-- Creating foreign key on [IDExamen] in table 'QuestionInExam'
ALTER TABLE [dbo].[QuestionInExam]
ADD CONSTRAINT [FK_QuestionInExam_Examen]
    FOREIGN KEY ([IDExamen])
    REFERENCES [dbo].[Examen]
        ([IDExamen])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Id] in table 'Pregunta'
ALTER TABLE [dbo].[Pregunta]
ADD CONSTRAINT [FK_Pregunta_Usuario]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[Usuario]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Pregunta_Usuario'
CREATE INDEX [IX_FK_Pregunta_Usuario]
ON [dbo].[Pregunta]
    ([Id]);
GO

-- Creating foreign key on [IDPregunta] in table 'QuestionInExam'
ALTER TABLE [dbo].[QuestionInExam]
ADD CONSTRAINT [FK_QuestionInExam_Pregunta]
    FOREIGN KEY ([IDPregunta])
    REFERENCES [dbo].[Pregunta]
        ([IDPregunta])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_QuestionInExam_Pregunta'
CREATE INDEX [IX_FK_QuestionInExam_Pregunta]
ON [dbo].[QuestionInExam]
    ([IDPregunta]);
GO

-- Creating foreign key on [ID_Pregunta] in table 'RespuestasErroneas'
ALTER TABLE [dbo].[RespuestasErroneas]
ADD CONSTRAINT [FK_RespuestasErroneas_Pregunta]
    FOREIGN KEY ([ID_Pregunta])
    REFERENCES [dbo].[Pregunta]
        ([IDPregunta])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_RespuestasErroneas_Pregunta'
CREATE INDEX [IX_FK_RespuestasErroneas_Pregunta]
ON [dbo].[RespuestasErroneas]
    ([ID_Pregunta]);
GO

-- Creating foreign key on [Carrera_Siglas] in table 'CarreraTieneTema'
ALTER TABLE [dbo].[CarreraTieneTema]
ADD CONSTRAINT [FK_CarreraTieneTema_Carrera]
    FOREIGN KEY ([Carrera_Siglas])
    REFERENCES [dbo].[Carrera]
        ([Siglas])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Tema_ClaveTema] in table 'CarreraTieneTema'
ALTER TABLE [dbo].[CarreraTieneTema]
ADD CONSTRAINT [FK_CarreraTieneTema_Tema]
    FOREIGN KEY ([Tema_ClaveTema])
    REFERENCES [dbo].[Tema]
        ([ClaveTema])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CarreraTieneTema_Tema'
CREATE INDEX [IX_FK_CarreraTieneTema_Tema]
ON [dbo].[CarreraTieneTema]
    ([Tema_ClaveTema]);
GO

-- Creating foreign key on [Examen_IDExamen] in table 'TemaEstaEnExamen'
ALTER TABLE [dbo].[TemaEstaEnExamen]
ADD CONSTRAINT [FK_TemaEstaEnExamen_Examen]
    FOREIGN KEY ([Examen_IDExamen])
    REFERENCES [dbo].[Examen]
        ([IDExamen])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Tema_ClaveTema] in table 'TemaEstaEnExamen'
ALTER TABLE [dbo].[TemaEstaEnExamen]
ADD CONSTRAINT [FK_TemaEstaEnExamen_Tema]
    FOREIGN KEY ([Tema_ClaveTema])
    REFERENCES [dbo].[Tema]
        ([ClaveTema])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_TemaEstaEnExamen_Tema'
CREATE INDEX [IX_FK_TemaEstaEnExamen_Tema]
ON [dbo].[TemaEstaEnExamen]
    ([Tema_ClaveTema]);
GO

-- Creating foreign key on [Pregunta_IDPregunta] in table 'TemaTienePregunta'
ALTER TABLE [dbo].[TemaTienePregunta]
ADD CONSTRAINT [FK_TemaTienePregunta_Pregunta]
    FOREIGN KEY ([Pregunta_IDPregunta])
    REFERENCES [dbo].[Pregunta]
        ([IDPregunta])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Tema_ClaveTema] in table 'TemaTienePregunta'
ALTER TABLE [dbo].[TemaTienePregunta]
ADD CONSTRAINT [FK_TemaTienePregunta_Tema]
    FOREIGN KEY ([Tema_ClaveTema])
    REFERENCES [dbo].[Tema]
        ([ClaveTema])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_TemaTienePregunta_Tema'
CREATE INDEX [IX_FK_TemaTienePregunta_Tema]
ON [dbo].[TemaTienePregunta]
    ([Tema_ClaveTema]);
GO

-- Creating foreign key on [Alumno_Matricula] in table 'AlumnoExamen'
ALTER TABLE [dbo].[AlumnoExamen]
ADD CONSTRAINT [FK_AlumnoExamen_Alumno]
    FOREIGN KEY ([Alumno_Matricula])
    REFERENCES [dbo].[Alumno]
        ([Matricula])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Examen_IDExamen] in table 'AlumnoExamen'
ALTER TABLE [dbo].[AlumnoExamen]
ADD CONSTRAINT [FK_AlumnoExamen_Examen]
    FOREIGN KEY ([Examen_IDExamen])
    REFERENCES [dbo].[Examen]
        ([IDExamen])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AlumnoExamen_Examen'
CREATE INDEX [IX_FK_AlumnoExamen_Examen]
ON [dbo].[AlumnoExamen]
    ([Examen_IDExamen]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------
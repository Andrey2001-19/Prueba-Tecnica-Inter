-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Servidor: 127.0.0.1
-- Tiempo de generación: 18-10-2026 a las 05:26:22
-- Versión del servidor: 10.4.32-MariaDB
-- Versión de PHP: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de datos: `pruebatecnicainter`
--

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `estudiantematerias`
--

CREATE TABLE `estudiantematerias` (
  `EstudianteId` int(11) NOT NULL,
  `MateriaId` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `estudiantematerias`
--

INSERT INTO `estudiantematerias` (`EstudianteId`, `MateriaId`) VALUES
(24, 1),
(24, 4),
(24, 5),
(25, 1),
(25, 4),
(25, 5);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `estudiantes`
--

CREATE TABLE `estudiantes` (
  `Id` int(11) NOT NULL,
  `Nombre` varchar(100) NOT NULL,
  `Correo` varchar(100) NOT NULL,
  `TotalCreditos` int(11) DEFAULT 9
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `estudiantes`
--

INSERT INTO `estudiantes` (`Id`, `Nombre`, `Correo`, `TotalCreditos`) VALUES
(24, 'asdsad', 'asdasdsad', 9),
(25, 'dasdsa', 'asdsadsad', 9);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `materias`
--

CREATE TABLE `materias` (
  `Id` int(11) NOT NULL,
  `Nombre` varchar(100) NOT NULL,
  `Creditos` int(11) DEFAULT 3,
  `ProfesorId` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `materias`
--

INSERT INTO `materias` (`Id`, `Nombre`, `Creditos`, `ProfesorId`) VALUES
(1, 'Matemáticas Básicas', 3, 1),
(2, 'Física Mecánica', 3, 1),
(3, 'Programación I', 3, 2),
(4, 'Bases de Datos I', 3, 2),
(5, 'Inglés Técnico', 3, 3),
(6, 'Ética Profesional', 3, 3),
(7, 'Contabilidad Financiera', 3, 4),
(8, 'Administración de Empresas', 3, 4),
(9, 'Redes de Computadores', 3, 5),
(10, 'Seguridad Informática', 3, 5);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `profesores`
--

CREATE TABLE `profesores` (
  `Id` int(11) NOT NULL,
  `Nombre` varchar(100) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `profesores`
--

INSERT INTO `profesores` (`Id`, `Nombre`) VALUES
(1, 'Profesor Carlos Pérez'),
(2, 'Profesora Ana Gómez'),
(3, 'Profesor Luis Rodríguez'),
(4, 'Profesora María Torres'),
(5, 'Profesor Jorge Ramírez');

--
-- Índices para tablas volcadas
--

--
-- Indices de la tabla `estudiantematerias`
--
ALTER TABLE `estudiantematerias`
  ADD PRIMARY KEY (`EstudianteId`,`MateriaId`),
  ADD KEY `MateriaId` (`MateriaId`);

--
-- Indices de la tabla `estudiantes`
--
ALTER TABLE `estudiantes`
  ADD PRIMARY KEY (`Id`);

--
-- Indices de la tabla `materias`
--
ALTER TABLE `materias`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `ProfesorId` (`ProfesorId`);

--
-- Indices de la tabla `profesores`
--
ALTER TABLE `profesores`
  ADD PRIMARY KEY (`Id`);

--
-- AUTO_INCREMENT de las tablas volcadas
--

--
-- AUTO_INCREMENT de la tabla `estudiantes`
--
ALTER TABLE `estudiantes`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=26;

--
-- AUTO_INCREMENT de la tabla `materias`
--
ALTER TABLE `materias`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=11;

--
-- AUTO_INCREMENT de la tabla `profesores`
--
ALTER TABLE `profesores`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- Restricciones para tablas volcadas
--

--
-- Filtros para la tabla `estudiantematerias`
--
ALTER TABLE `estudiantematerias`
  ADD CONSTRAINT `estudiantematerias_ibfk_1` FOREIGN KEY (`EstudianteId`) REFERENCES `estudiantes` (`Id`) ON DELETE CASCADE,
  ADD CONSTRAINT `estudiantematerias_ibfk_2` FOREIGN KEY (`MateriaId`) REFERENCES `materias` (`Id`);

--
-- Filtros para la tabla `materias`
--
ALTER TABLE `materias`
  ADD CONSTRAINT `materias_ibfk_1` FOREIGN KEY (`ProfesorId`) REFERENCES `profesores` (`Id`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;

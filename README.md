# 📚 Sistema de Registro de Estudiantes - Prueba Técnica

Aplicación web full-stack desarrollada como prueba técnica para la gestión de inscripción de estudiantes, garantizando el cumplimiento de reglas de negocio académicas (límite de créditos y validación de profesores únicos).

## Tecnologías Utilizadas

- **Backend:** .NET Core 8.0 Web API, Entity Framework Core, LINQ.
- **Frontend:** Angular (Standalone Components, SSR/Vite), TypeScript, HTML5, CSS3.
- **Base de Datos:** MySQL.

---

## Reglas de Negocio Implementadas
1. **Límite de Créditos:** Cada estudiante debe inscribir exactamente 3 materias (9 créditos en total).
2. **Restricción de Profesores:** No se pueden seleccionar dos materias dictadas por el mismo profesor; cada curso debe tener un docente diferente.
3. **Validación de Datos:** Nombres y correos electrónicos obligatorios y validados tanto en el cliente como en el servidor.
4. **Visualización de Compañeros:** Permite consultar en tiempo real un listado detallado (con nombres y correos) de los compañeros que comparten una misma clase.

---

## Instrucciones de Ejecución Local

### 1. Base de Datos (MySQL)
- Importa el archivo SQL ubicado en la carpeta `Bd/`.
- Configura tu cadena de conexión en el archivo `appsettings.json` del proyecto Backend.

### 2. Backend (.NET Core)
1. Abre una terminal y navega hasta la carpeta `Back/`.
2. Ejecuta el siguiente comando para iniciar el servidor:
   dotnet run

### 3. FrontEnd (Angular)
1. Abre una terminal y navega hasta la carpeta Front/
2. Instala las dependencias `npm install`
3. Inicia la aplicación `ng serve`
4. Abre tu navegador en http://localhost:4200

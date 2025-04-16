# 🍽️ Hipódromo Reservas

Aplicación para gestionar reservas de mesas del restaurante TUCSON,
ubicado en el **Hipódromo de Palermo**.
-------------------------------------------------------------------------------------------

### El sistema incluye:

- Gestión de reservas según categoría de cliente
- Asignación automática de mesas según disponibilidad.
- Lista de espera cuando no hay mesas libres.
- Reasignación automática de mesas al cancelar una reserva.

-------------------------------------------------------------------------------------------

### Reglas de negocio consideradas en el desarrollo:

- Las reservas tienen una duracion de 1.30hs.
- Al momento de reservar, el horario no puede solapar horarios existentes teniendo en cuenta
la duración mencionada.
- La reasignación se hace primero por categoria del cliente, luego por la fecha en que 
se agregó a la lista de espera.

-------------------------------------------------------------------------------------------

## 📦 Tecnologías utilizadas

### Backend			### Frontend
- .NET 8.0			- Angular: 19.2.6
- FluentAssertions		- PrimeNG
- Moq				- Tailwind
- XUnit

-------------------------------------------------------------------------------------------

## 🚀 Instrucciones para correr el proyecto

###. Requisitos
- Visual Studio 
- Visual Studio Code

-------------------------------------------------------------------------------------------

### 1. Clonar el repositorio
	- Elegir una carpeta de destino, abrir una consola con cmd y ejecutar el comando:
		git clone https://github.com/fedepuleio/HipodromoReservas.git

-------------------------------------------------------------------------------------------

### 2. Para correr el front
	- Ir a la carpeta HipodromoReservas > hipodromo-web, dar clic derecho y elegir la opción "Abrir con Code".
	- Abrir la consola con CTRL + Ñ.
	- Ejecutar los comandos:
		- npm i
		- ng serve
	- Una vez iniciada la aplicación, estará disponible en la dirección http://localhost:4200/.

-------------------------------------------------------------------------------------------

### 3. Para correr la API
	- Ir a la carpeta HipodromoReservas > HipodromoReservas, 
	  y abrir la solución HipodromoReservas.sln en Visual Studio.
	- Seleccionar el proyecto HipodromoReservas.Api como proyecto de inicio. 
  	  (clic derecho sobre el proyecto y elegir la opción "establecer como proyecto de inicio.")
	- Para ejecutar el proyecto, seleccionar de las opciones "https".
	- Ejecutar con F5.
	- Una vez iniciada la aplicación, estará disponible en la direccion http://localhost:5298.

-------------------------------------------------------------------------------------------

### 4. Arquitectura 

Api
	HipodromoReservas/
	├── HipodromoReservas.Api/         	# Proyecto API .NET
	├── HipodromoReservas.Application/ 	# Lógica de aplicación
	├── HipodromoReservas.Domain/      	# Entidades de dominio y lógica de negocio
	├── HipodromoReservas.Infrastructure/ 	# Repositorios y configuración EF
	├── HipodromoReservas.Tests/       	# Tests unitarios

Front
	hipodromo-web/         # Proyecto Angular
	├─ src/
	     ├─ environments/
    	     ├─ app/
     	     	  ├─ components/
     	     	  ├─ models/      
		  ├─ pages/       
		  ├─ services/  

-------------------------------------------------------------------------------------------
****************************
**** Proyecto realizado ****
****	    por         ****
****   Federico Puleio  ****
**** fgpuleio@gmail.com ****
****       2025         ****
****************************

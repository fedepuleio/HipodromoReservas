# 🍽️ Hipódromo Reservas

Aplicación para gestionar reservas de mesas del restaurante TUCSON,
ubicado en el **Hipódromo de Palermo**.
  
El sistema incluye:

- Gestión de reservas según categoría de cliente
- Asignación automática de mesas según disponibilidad.
- Lista de espera cuando no hay mesas libres.
- Reasignación automática de mesas al cancelar una reserva.

Reglas de negocio consideradas en el desarrollo:
- Las reservas tienen una duracion de 1.30hs.
- Al momento de reservar, el horario no puede solapar horarios existentes teniendo en cuenta
la duración mencionada.
- La reasignación se hace primero por categoria del cliente, luego por 

---

## 📦 Tecnologías utilizadas

### Backend
- .NET 8.0
- FluentAssertions
- Moq
- XUnit

### Frontend
- Angular: 19.2.6
- PrimeNG
- Tailwind

---

## 🚀 Instrucciones para correr el proyecto

### 1. Clonar el repositorio

	Elegir una carpeta de destino y correr el comando 
	git clone https://github.com/fedepuleio/HipodromoReservas.git

### 2. Para correr el front
	cd hipodromo-web 
	npm i 
	ng serve


### 3. Para correr la API
	Abrir la solucion 


### 4. Arquitectura 

Api
	HipodromoReservas/
	├── HipodromoReservas.Api/         # Proyecto API .NET
	├── HipodromoReservas.Application/ # Lógica de aplicación
	├── HipodromoReservas.Domain/      # Entidades de dominio y lógica de negocio
	├── HipodromoReservas.Infrastructure/ # Repositorios y configuración EF
	├── HipodromoReservas.Tests/       # Tests unitarios

Front
	hipodromo-web/         # Proyecto Angular (frontend)
	├─ src/
	     ├─ environments/
    	     ├─ app/
     	     	  ├─ components/
     	     	  ├─ models/      
		  ├─ pages/       
		  ├─ services/   
     	    

Proyecto realizado por Federico Puleio.
fgpuleio@gmail.com
2025
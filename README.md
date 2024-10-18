# underground-parking-system-BACKEND

This project is created based on the Project Definision (you can check the full assignment below).
The main purpose is to help the employees at the [P3 company](https://www.p3-group.com/)
The project is structured into two distinct sections: the dynamic and robust backend, complemented by the sleek and intuitive [frontend](https://github.com/lachezar1/Underground-parking-system-FRONTEND).

### Project Definition: Underground Parking Spaces Management System
#### 1. Overview: 
The Underground Parking Spaces Management System is a web-based application designed to streamline the management and reservation of parking spaces in an underground parking facility. The system consists of a React-based web application for user interaction, a web API to handle backend logic and communication with the database, and a database schema to store and manage parking space reservations. Additionally, the web application will incorporate SVG (Scalable Vector Graphics) to allow users to visually select specific parking spots.
#### 2. Components:
##### 2.1. Web Application (React): 
The user-facing component of the system is a React-based web application. It provides an intuitive interface for users to log in, view available parking spaces, reserve a space for a specific time period (up to 8 hours), and manage their reservations. The application will feature responsive design to ensure compatibility across various devices.
##### 2.2. Web API: 
The web API serves as the backend logic layer of the system. It exposes endpoints to handle user authentication, parking space availability, reservation creation, modification, and cancellation. The API will be developed using a technology stack suitable for building robust and scalable web services, such as Node.js with Express framework.
##### 2.3. Database Schema:
The database schema stores all relevant data for the system, including user information, parking space details, and reservation records. The schema will be designed to efficiently manage data relationships and support fast querying for reservation status and availability. A relational database management system like PostgreSQL will be used to implement the schema.
#### 3. Functionality:
##### 3.1. User Authentication:
Users will be required to log in to the web application using their credentials (e.g., username and password) to access the reservation features. Authentication will be implemented securely to protect user accounts and data.
##### 3.2. Parking Space Reservation:
Users can view the available parking spaces in the underground facility and reserve a space for a specific time period, with a maximum duration of 8 hours. Upon successful reservation, the corresponding parking space will be marked as unavailable for the reserved duration.
##### 3.3. Reservation Management: 
Users can view their active reservations, modify reservation details (e.g., time period), and cancel reservations if needed. The system will handle updates to parking space availability based on reservation modifications or cancellations.
##### 3.4. SVG Parking Spot Selection: 
The web application will utilize SVG to visually represent the layout of the underground parking facility. Users will be able to interact with the SVG graphic to select a specific parking spot for their reservation. This interactive feature enhances the user experience by providing a visual representation of available parking spaces.
#### 4. Technologies:
 - Frontend: React, HTML5, CSS3, JavaScript, SVG
 - Backend: Node.js, Express.js / Java / .NET
 - Database: PostgreSQL / MySql / MSSQL
 - Authentication: Basic
#### 5. Additional Features (Optional):
Real-time availability updates: Implementing real-time updates to parking space availability to provide users with instant feedback on space availability.
Payment Integration: Integrating payment processing functionality for paid parking reservations.
Notifications: Sending email or SMS notifications to users for reservation confirmations, reminders, or cancellations.
#### 6. Future Considerations: 
The system can be further enhanced with features such as parking space occupancy monitoring using sensors, integration with external calendar applications for reservation scheduling, and analytics for parking space utilization.
#### 7. Deployment: 
The system will be deployed on a cloud platform like AWS or Azure to ensure scalability, reliability, and accessibility from anywhere with an internet connection.
This project aims to improve the efficiency of managing parking spaces in underground facilities while providing users with a convenient and reliable reservation system. The integration of SVG for parking spot selection enhances the user experience by providing a visual representation of available parking spaces within the facility.

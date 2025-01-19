# Gift Shop Management System

## Project Description

The Gift Shop Management System is designed to help a gift shop owner manage their inventory efficiently. The system provides a product management interface where users can:

- **View Products**: Display a list of all products with pagination.
- **Add Products**: Add new products to the inventory.
- **Edit Products**: Update details of existing products.
- **Delete Products**: Remove products from the inventory.
- **Upload/Change/Delete Images**: Manage product images.
- **Search and Sort**: Perform quick searches by product name or code and sort by any field.

---

## Technical Implementation

### Backend
- **Platform**: ASP.NET
- **Database**: Internal MDF file utilizing SQL Server.

### Frontend
- **Framework Options**: VUE.JS
- **Design**: Element UI

### Data Communication
- All data exchange between the client and server is handled using AJAX to ensure a seamless and dynamic user experience.

---

## Features Overview

### Product Management
1. **View Products**:
   - Pagination for browsing through the product list.
2. **Add Products**:
   - Input fields for product code, name, description, start sale date, and image.
3. **Edit Products**:
   - Modify existing product details.
4. **Delete Products**:
   - Option to remove products permanently.

### Search and Sorting
- Search by product name or code.
- Sort products by any field.

### Image Management
- Upload, change, or delete images for each product.

### Design and User Experience
- Modern and clean design implemented with Element Plus.

---

## Installation and Setup

1. **Clone the Repository**:
   ```bash
   git clone <repository_url>
   ```

2. **Open the Project**:
   - Launch the solution in Visual Studio 2019 Community Edition.

3. **Setup Database**:
   - The MDF file in the src: `GiftShopManager/GiftShopManager/App_Data/GiftShopDatabase.mdf`
   - Execute the included SQL script to create necessary tables and stored procedures.

4. **Run the Application**:
   - Start the ASP.NET application.
   - **Use localhost `https://44335`**

---

## Notes
- The project emphasizes simplicity with a minimalistic single-page client and a single service on the server side.
- No security measures (authentication/authorization) are implemented as per the project requirements.

---


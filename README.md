# 📦 Documentación de la API - Carrito de Compras

## 📘 Introducción

Esta documentación técnica describe el funcionamiento de la API RESTful del sistema de carrito de compras. Permite a los usuarios:

- Listar productos disponibles.
- Agregar productos al carrito.
- Visualizar el contenido del carrito.
- Calcular totales de compra.
- Simular un proceso de checkout (confirmación y pago).

---

## 🛒 Carrito de Compras
### 1. Obtener Productos del Carrito

- **Método:** `GET`  
- **Ruta:** `/api/cart/:userId`

**Descripción:**  
Devuelve todos los productos del carrito de un usuario específico, incluyendo cantidades y el total a pagar.

**Respuesta:**
```json
{
  "message": "Cart retrieved successfully",
  "items": [
    {
      "id": 10,
      "quantity": 2,
      "product": {
        "id": 1,
        "name": "Camisa Azul",
        "price": 29.99,
        "imageUrl": "https://..."
      }
    }
  ],
  "totalItems": 2,
  "totalPrice": 59.98
}
```

### 2. Agregar Producto al Carrito

- **Método:** `POST`  
- **Ruta:** `/api/cart/add`

**Descripción:**  
Agrega un producto al carrito de un usuario.

**Cuerpo de la solicitud:**
```json
{
  "userId": "abc123",
  "productId": 101,
  "quantity": 1
}
```

**Respuesta:**
```json
{
  "message": "Producto agregado al carrito",
  "success": true
}
```


### 3. Actualizar Cantidad en el Carrito

- **Método:** `PUT`  
- **Ruta:** `/api/cart/quantity`

**Descripción:**  
Actualiza la cantidad de un producto específico en el carrito de un usuario.

 delta > 0: Aumenta la cantidad
 delta < 0: Disminuye la cantidad
 Si la cantidad llega a 0 o menos, el producto puede eliminarse automáticamente.

**Cuerpo de la solicitud:**
```json
{
  "userId": 1,
  "productId": 101,
  "delta": 1
}
```

**Respuesta:**
```json
{
  "message": "Cantidad actualizada correctamente",
  "success": true,
  "error": false
}
```

### 4. Eliminar Producto del Carrito

  - **Método:** `DELETE`  
  - **Ruta:** `/api/cart/:userId/:productId`

**Descripción:**  
  Elimina un producto específico del carrito de un usuario.


**Cuerpo de la solicitud:**
```json
  {
    "userId": "1",
    "productId": "1"
  }
```

**Respuesta:**
```json
{
  "message": "Producto eliminado del carrito",
  "success": true
}
```

### 5. Confirmar Pago (Checkout)

  - **Método:** `POST`  
  - **Ruta:** `/api/cart/checkout?userId`

**Descripción:**  
  Simula un pago exitoso. Se espera que el carrito ya esté cargado previamente.

**Cuerpo de la solicitud:**
```json
  {
    "userId": "1",
  }
```

**Respuesta:**
```json
{
  "message": "Payment successful",
  "success": true,
  "error": false
}
```

## 🛒 Productos
### 1. Obtener Productos

- **Método:** `GET`  
- **Ruta:** `/api/products`

**Descripción:**  
  Devuelve una lista con todos los productos disponibles en el sistema.

**Respuesta:**
```json
[
  {
    "id": 1,
    "name": "Camisa Azul",
    "description": "Camisa 100% algodón",
    "price": 29.99,
    "imageUrl": "https://..."
  },
  {
    "id": 2,
    "name": "Camisa Roja",
    "description": "Camisa 100% algodón",
    "price": 29.99,
    "imageUrl": "https://..."
  }
]
```

### 2. Obtener Producto por ID

  - **Método:** `GET`  
  - **Ruta:** `/api/products/:productId`

**Descripción:**  
  Obtiene los detalles de un producto específico.

**Cuerpo de la solicitud:**
```json
  {
    "productId": "1",
  }
```

**Respuesta:**
```json
{
  "id": 1,
  "name": "Laptop Dell XPS 15",
  "description": "Laptop potente para trabajo y gaming.",
  "price": 1500,
  "imageUrl": "/images/laptop1.jpg"
}
```

### Usuarios
### 1. Registrar Usuario

  - **Método:** `POST`  
  - **Ruta:** `/api/user/register`

**Descripción:**  
  Registra un nuevo usuario en el sistema.

**Cuerpo de la solicitud:**
```json
{
  "name": "string",
  "username": "string",
  "password": "string"
}
```

**Respuesta:**
```json
{
  "message": "user successfully registered",
  "data": {
    "id": 2,
    "userName": "string",
    "name": "string"
  },
  "error": false,
  "success": true
}
```

### 2. Login de Usuario

  - **Método:** `POST`  
  - **Ruta:** `/api/user/login`

**Descripción:**  
  Permite iniciar sesión a un usuario existente.

**Cuerpo de la solicitud:**
```json
{
  "username": "string",
  "password": "string"
}
```

**Respuesta:**
```json
{
  "message": "Login successfully",
  "data": {
    "id": 2,
    "userName": "string",
    "name": "string"
  },
  "error": false,
  "success": true
}
```

### 3. Obtener Usuario Específico

  - **Método:** `GET`  
  - **Ruta:** `/api/user/:userId`

**Descripción:**  
  Obtiene los datos de un solo usuario del sistema a partir de su userId.

**Cuerpo de la solicitud:**
```json
{
  "userId": "2"
}
```

**Respuesta:**
```json
{
  "message": "User found",
  "data": {
    "id": 2,
    "userName": "string",
    "name": "string"
  },
  "error": false,
  "success": true
}
```

### 4. Obtener Todos los Usuarios

  - **Método:** `GET`  
  - **Ruta:** `/api/user`

**Descripción:**  
  Devuelve la lista de todos los usuarios registrados en el sistema.


**Respuesta:**
```json
{
  "message": "Users",
  "data": [
    {
      "id": 2,
      "userName": "string",
      "name": "string"
    },
    {
      "id": 3,
      "userName": "string1",
      "name": "string"
    }
  ],
  "error": false,
  "success": true
}
```

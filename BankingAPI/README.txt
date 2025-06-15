Prueba del APi Swagger
datos para pruebas 
Cuenta:    CR0120129837891273897129
idCliente: 1


1 Clientes:    

 GET   /api/Clientes  - boton Probar  y luego ejecutar -   Obtiene lista de clientes
 
 Post  /api/Clientes  - botn probar y llenar los valores en el json
                      {
  "id": 0,
  "nombre": "string",
  "fechaNacimiento": "string",
  "sexo": "string",
  "ingresos": 0
}

Get /api/Clientes/{id}     - boton probar - ingresar id del clientes

Put /api/Clientes/{id}     - boton probar - ingresar valores en la estructura json 


2 CuentasBancarias

/api/CuentasBancarias    Obtiene lista de cuentas
/api/CuentasBancarias    Crea cuentas Bancarias
/api/CuentasBancarias/numero/{numeroCuenta}     edita por numero de cuenta
/api/CuentasBancarias/saldo/{numeroCuenta}      obtiene saldo de cuenta

3 Transacciones

/api/Transacciones/transaccion/{id}                consulta por id de transaccion/
/api/Transacciones/por-cuenta/{numeroCuenta}       consulta por numero de cuenta/

/api/Transacciones/registrar    boton probar  ingres los valores en la estructura

{
  "numeroCuenta": "string",
  "tipo": "string",           (deposito o retiro)
  "monto": 0
}                  

/api/Transacciones/resumen/{numeroCuenta}         devuelve el resumen por numero de cuenta 

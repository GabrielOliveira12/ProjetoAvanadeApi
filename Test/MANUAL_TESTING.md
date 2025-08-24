# Manual de Teste da API

Este documento contém exemplos de como testar manualmente os endpoints da API usando ferramentas como curl, Postman ou similar.

## Iniciando a API

```bash
cd /home/gabriel/ProjetoAvanadeApi
dotnet run --project Api/
```

A API estará disponível em: `http://localhost:5000`

## Exemplos de Requests

### 1. Testar página inicial
```bash
curl -X GET http://localhost:5000/
```

**Resposta esperada:**
```json
{
  "title": "Projeto Avanade API",
  "doc": "/swagger"
}
```

### 2. Login de Administrador
```bash
curl -X POST http://localhost:5000/administradores/login \
  -H "Content-Type: application/json" \
  -d '{
    "email": "useradm@teste.com",
    "senha": "123456"
  }'
```

**Resposta esperada:**
```json
{
  "email": "useradm@teste.com",
  "perfil": "Adm",
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}
```

### 3. Criar Novo Administrador (requer token)
```bash
curl -X POST http://localhost:5000/administradores \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer SEU_TOKEN_AQUI" \
  -d '{
    "email": "novo@admin.com",
    "senha": "senha123",
    "perfil": "user"
  }'
```

### 4. Listar Administradores (requer token)
```bash
curl -X GET http://localhost:5000/administradores \
  -H "Authorization: Bearer SEU_TOKEN_AQUI"
```

### 5. Criar Veículo (requer token)
```bash
curl -X POST http://localhost:5000/veiculos \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer SEU_TOKEN_AQUI" \
  -d '{
    "nome": "Civic",
    "marca": "Honda", 
    "ano": 2020
  }'
```

### 6. Listar Veículos (requer token)
```bash
curl -X GET http://localhost:5000/veiculos \
  -H "Authorization: Bearer SEU_TOKEN_AQUI"
```

### 7. Buscar Veículo por ID (requer token)
```bash
curl -X GET http://localhost:5000/veiculos/1 \
  -H "Authorization: Bearer SEU_TOKEN_AQUI"
```

### 8. Atualizar Veículo (requer token admin)
```bash
curl -X PUT http://localhost:5000/veiculos/1 \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer SEU_TOKEN_AQUI" \
  -d '{
    "nome": "Civic Updated",
    "marca": "Honda",
    "ano": 2023
  }'
```

### 9. Deletar Veículo (requer token admin)
```bash
curl -X DELETE http://localhost:5000/veiculos/1 \
  -H "Authorization: Bearer SEU_TOKEN_AQUI"
```

## Testando Validações

### Testar dados inválidos:
```bash
curl -X POST http://localhost:5000/veiculos \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer SEU_TOKEN_AQUI" \
  -d '{
    "nome": "",
    "marca": "",
    "ano": 1940
  }'
```

**Resposta esperada (400 Bad Request):**
```json
{
  "messages": [
    "O nome do veículo é obrigatório.",
    "A marca do veículo é obrigatória.", 
    "O ano do veículo é muito antigo."
  ]
}
```

## Acessando Swagger

Visite: `http://localhost:5000/swagger`

O Swagger UI permite testar todos os endpoints de forma interativa com uma interface gráfica.

## Códigos de Status HTTP

- **200 OK** - Sucesso
- **201 Created** - Recurso criado com sucesso
- **400 Bad Request** - Dados inválidos
- **401 Unauthorized** - Token não fornecido ou inválido
- **403 Forbidden** - Sem permissão para acessar o recurso
- **404 Not Found** - Recurso não encontrado
- **500 Internal Server Error** - Erro interno do servidor

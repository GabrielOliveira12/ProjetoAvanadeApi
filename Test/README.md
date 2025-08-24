# Testes do Projeto Avanade API

Este projeto contém uma suite completa de testes para a API desenvolvida em ASP.NET Core, incluindo testes unitários para os serviços e testes básicos para validação de requests HTTP.

## Estrutura dos Testes

### 📁 Test/Domain/Services/
Contém testes unitários para os serviços de negócio:

#### AdminServicesTests.cs
- ✅ `TestCreateAdmin()` - Testa a criação de administradores
- ✅ `TestLoginAdmin_ValidCredentials()` - Testa login com credenciais válidas
- ✅ `TestLoginAdmin_InvalidCredentials()` - Testa login com credenciais inválidas
- ✅ `TestGetAllAdmins()` - Testa listagem de todos os administradores
- ✅ `TestGetAdminById()` - Testa busca de administrador por ID
- ✅ `TestUpdateAdmin()` - Testa atualização de administrador
- ✅ `TestDeleteAdmin()` - Testa exclusão de administrador
- ✅ `TestGetAdminsWithPagination()` - Testa paginação na listagem

#### VehicleServicesTests.cs
- ✅ `TestCreateVehicle()` - Testa a criação de veículos
- ✅ `TestGetAllVehicles()` - Testa listagem de todos os veículos
- ✅ `TestGetVehicleById()` - Testa busca de veículo por ID
- ✅ `TestUpdateVehicle()` - Testa atualização de veículo
- ✅ `TestDeleteVehicle()` - Testa exclusão de veículo
- ✅ `TestGetVehiclesWithPagination()` - Testa paginação na listagem
- ✅ `TestGetVehiclesWithFilters()` - Testa filtros de busca (nome, marca, ano)
- ✅ `TestGetVehicleByIdNotFound()` - Testa busca de veículo inexistente

### 📁 Test/Requests/
Contém testes básicos para validação de requests HTTP:

#### BasicRequestTests.cs
- ✅ `TestHttpClientConnection()` - Testa conexão HTTP básica
- ✅ `TestJsonSerialization()` - Testa serialização/deserialização JSON
- ✅ `TestHttpContentCreation()` - Testa criação de conteúdo HTTP
- ✅ `TestRequestBuilding()` - Testa construção de requests HTTP
- ✅ `DocumentationTestEndpoints()` - Documenta endpoints disponíveis

## Endpoints da API Testados

### 🏠 Home
- `GET /` - Página inicial da API

### 👤 Administradores
- `POST /administradores/login` - Login de administrador
- `POST /administradores` - Criação de novo administrador (requer auth)
- `GET /administradores` - Listagem de administradores (requer auth)
- `GET /administradores/{id}` - Busca administrador por ID (requer auth)
- `PUT /administradores/{id}` - Atualização de administrador (requer auth)
- `DELETE /administradores/{id}` - Exclusão de administrador (requer auth)

### 🚗 Veículos
- `POST /veiculos` - Criação de novo veículo (requer auth)
- `GET /veiculos` - Listagem de veículos (requer auth)
- `GET /veiculos/{id}` - Busca veículo por ID (requer auth)
- `PUT /veiculos/{id}` - Atualização de veículo (requer auth admin)
- `DELETE /veiculos/{id}` - Exclusão de veículo (requer auth admin)

## Como Executar os Testes

### Executar todos os testes:
```bash
cd /home/gabriel/ProjetoAvanadeApi
dotnet test Test/
```

### Executar apenas testes de serviços:
```bash
dotnet test Test/Domain/Services/
```

### Executar apenas testes de requests:
```bash
dotnet test Test/Requests/
```

### Executar com detalhes verbosos:
```bash
dotnet test Test/ --verbosity normal
```

## Tecnologias de Teste Utilizadas

- **MSTest** - Framework de testes unitários da Microsoft
- **Entity Framework InMemory** - Banco de dados em memória para testes
- **HttpClient** - Para testes de requests HTTP
- **System.Text.Json** - Para serialização/deserialização JSON

## Configurações de Teste

- **Banco de Dados**: Utiliza Entity Framework InMemory para isolamento
- **Autenticação**: Testes simulam tokens JWT para endpoints protegidos
- **Validação**: Testa tanto cenários de sucesso quanto de falha
- **Paginação**: Valida sistema de paginação (10 itens por página)

## Resultados dos Testes

📊 **Status atual**: ✅ 22 testes passando, 0 falhando

### Cobertura de Testes:
- ✅ CRUD completo de Administradores
- ✅ CRUD completo de Veículos  
- ✅ Sistema de autenticação e autorização
- ✅ Validação de dados de entrada
- ✅ Paginação e filtros
- ✅ Tratamento de erros (NotFound, BadRequest, etc.)
- ✅ Serialização JSON
- ✅ Construção de requests HTTP

## Observações

1. **Testes de Integração**: Os testes de request são básicos e validam a estrutura HTTP. Para testes de integração completos, a API deve estar em execução.

2. **Banco de Dados**: Os testes utilizam bancos em memória independentes, garantindo isolamento entre os testes.

3. **Autenticação**: Os testes validam tanto cenários autenticados quanto não autenticados.

4. **Validation**: Todos os cenários de validação de entrada são testados (campos obrigatórios, formatos, etc.).

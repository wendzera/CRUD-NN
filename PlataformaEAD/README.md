# Plataforma EAD - Sistema de Matrículas

Sistema completo de gestão educacional desenvolvido com ASP.NET Core MVC, implementando relacionamento N:N entre Alunos e Cursos através de uma entidade de Matrícula com dados específicos.

## 📋 Funcionalidades

### ✅ Gestão de Alunos
- ✓ CRUD completo (Create, Read, Update, Delete)
- ✓ Busca por nome, email ou telefone
- ✓ Validação de dados obrigatórios
- ✓ Visualização de matrículas por aluno
- ✓ Proteção contra exclusão de alunos com matrículas

### ✅ Gestão de Cursos
- ✓ CRUD completo com validações
- ✓ Busca por título ou descrição
- ✓ Controle de preço base e carga horária
- ✓ Visualização de alunos matriculados
- ✓ Proteção contra exclusão de cursos com matrículas

### ✅ Sistema de Matrículas (N:N)
- ✓ Relacionamento N:N com carga de dados
- ✓ Matrícula múltipla (um aluno em vários cursos)
- ✓ Controle de progresso (0-100%)
- ✓ Sistema de notas (0-10)
- ✓ Status: Ativo, Concluído, Cancelado
- ✓ Validação: Status "Concluído" requer progresso 100%
- ✓ Prevenção de matrículas duplicadas

### 🎨 Interface e UX
- ✓ Design responsivo com Bootstrap 5
- ✓ Sidebar com navegação intuitiva
- ✓ Dashboard com estatísticas
- ✓ Alertas de feedback para ações
- ✓ Formulários com validação em tempo real
- ✓ Barra de progresso visual
- ✓ Ícones Font Awesome

## 🛠 Tecnologias Utilizadas

- **ASP.NET Core 8.0 MVC** - Framework web
- **Entity Framework Core 8.0** - ORM
- **PostgreSQL** - Banco de dados
- **Bootstrap 5.3** - Framework CSS
- **Font Awesome 6.0** - Ícones
- **jQuery** - JavaScript

## 📊 Modelo de Dados

### Aluno
```csharp
- Id (int, PK)
- Nome (string, required, max 100)
- Email (string, optional, max 255, unique)
- Telefone (string, optional, max 20)
- Matriculas (ICollection<Matricula>)
```

### Curso
```csharp
- Id (int, PK)
- Titulo (string, required, max 200)
- Descricao (string, optional, max 1000)
- PrecoBase (decimal, precision 18,2)
- CargaHoraria (int, min 1)
- Matriculas (ICollection<Matricula>)
```

### Matricula (Entidade de Junção N:N)
```csharp
- AlunoId (int, PK composta)
- CursoId (int, PK composta)
- Data (DateTime, UTC timezone)
- PrecoPago (decimal, precision 18,2, min 0)
- Status (enum: Ativo, Concluido, Cancelado)
- Progresso (int, 0-100)
- NotaFinal (decimal?, precision 4,2, 0-10)
```

## ⚙️ Configurações do Entity Framework

### Mapeamentos Implementados
- **Chave Composta**: `HasKey(m => new { m.AlunoId, m.CursoId })`
- **Precisão Decimal**: `HasPrecision(18,2)` para valores monetários
- **Delete Behavior**: `OnDelete(DeleteBehavior.Restrict)` para preservar histórico
- **Timezone**: `timestamp with time zone` para dados UTC
- **Relacionamentos**: 1:N configurados corretamente

### Validações de Negócio
1. **Matrícula**: Pelo menos um curso deve ser selecionado
2. **Status Concluído**: Requer progresso = 100%
3. **Preço Pago**: Deve ser ≥ 0
4. **Progresso**: Entre 0 e 100
5. **Nota Final**: Entre 0 e 10 (opcional)
6. **Email Único**: Não permite emails duplicados

## 🚀 Como Executar

### Pré-requisitos
- .NET 8.0 SDK
- PostgreSQL
- Visual Studio 2022 ou VS Code

### Passos para Execução

1. **Clone o projeto**
```bash
git clone <repository-url>
cd PlataformaEAD
```

2. **Configure a string de conexão**
Edite `appsettings.json` e `appsettings.Development.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=PlataformaEAD;Username=seu_usuario;Password=sua_senha"
  }
}
```

3. **Instale as dependências**
```bash
dotnet restore
```

4. **Crie e aplique as migrações**
```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

5. **Execute o projeto**
```bash
dotnet run
```

6. **Acesse a aplicação**
Navegue para `https://localhost:7xxx` ou `http://localhost:5xxx`

## 📁 Estrutura do Projeto

```
PlataformaEAD/
├── Controllers/          # Controllers MVC
│   ├── HomeController.cs
│   ├── AlunosController.cs
│   ├── CursosController.cs
│   └── MatriculasController.cs
├── Data/                 # Contexto do EF Core
│   └── AppDbContext.cs
├── Models/              # Entidades de domínio
│   ├── Aluno.cs
│   ├── Curso.cs
│   └── Matricula.cs
├── ViewModels/          # ViewModels para formulários
│   └── MatriculaViewModel.cs
├── Views/               # Views Razor
│   ├── Home/
│   ├── Alunos/
│   ├── Cursos/
│   ├── Matriculas/
│   └── Shared/
├── wwwroot/             # Arquivos estáticos
│   ├── css/
│   └── js/
├── Program.cs           # Configuração da aplicação
├── PlataformaEAD.csproj # Arquivo do projeto
└── README.md           # Esta documentação
```

## 🎯 Funcionalidades Implementadas

### ✅ Requisitos Obrigatórios Atendidos

1. **Modelagem N:N com carga** ✓
   - Entidade Matricula como junção explícita
   - Dados específicos na entidade de junção

2. **CRUD Completo** ✓
   - Alunos: Create, Read, Update, Delete + Busca
   - Cursos: Create, Read, Update, Delete + Busca
   - Matrículas: Create, Read, Update, Delete

3. **Tratamento de Decimais** ✓
   - Precisão configurada (18,2)
   - Validações de intervalo

4. **Tratamento de Data/Hora** ✓
   - UTC timezone configurado
   - Campos timestamp with time zone

5. **Validações de Negócio** ✓
   - Status Concluído = Progresso 100%
   - Pelo menos um curso na matrícula
   - Todas as validações de campo

6. **Feedback de UX** ✓
   - Alerts de sucesso/erro
   - Validação em tempo real
   - Interface responsiva

## 💡 Destaques Técnicos

### Relacionamento N:N Avançado
- Implementação completa de entidade de junção
- Chave primária composta
- Dados específicos na tabela intermediária
- Navegação bidirecional

### Validações Inteligentes
- Validação customizada para status/progresso
- Prevenção de matrículas duplicadas
- Validação de email único
- Feedback visual em tempo real

### Interface Moderna
- Design responsivo mobile-first
- Componentes Bootstrap customizados
- Animações CSS suaves
- Dashboard com métricas

### Arquitetura Limpa
- Separação clara de responsabilidades
- ViewModels para formulários complexos
- Controllers organizados
- Configuração centralizada

## 🔧 Configurações de Desenvolvimento

### Ferramentas Necessárias
- Visual Studio 2022 17.8+ ou VS Code
- .NET 8.0 SDK
- PostgreSQL 14+
- Git

### Extensões Recomendadas (VS Code)
- C# for Visual Studio Code
- C# Extensions
- PostgreSQL Explorer
- Auto Rename Tag
- Prettier

### Comandos Úteis
```bash
# Restaurar pacotes
dotnet restore

# Build do projeto
dotnet build

# Executar em desenvolvimento
dotnet run

# Executar com hot reload
dotnet watch run

# Criar nova migration
dotnet ef migrations add NomeDaMigracao

# Aplicar migrations
dotnet ef database update

# Reverter migration
dotnet ef database update MigracaoAnterior

# Remover última migration
dotnet ef migrations remove
```

## 🛡️ Segurança e Boas Práticas

### Implementadas
- ✅ Proteção CSRF com AntiForgeryToken
- ✅ Validação de entrada server-side e client-side
- ✅ Sanitização de dados
- ✅ Tratamento de exceções
- ✅ Configuração de ambiente

### Recomendações para Produção
- [ ] Implementar autenticação e autorização
- [ ] Configurar HTTPS obrigatório
- [ ] Implementar rate limiting
- [ ] Adicionar logs estruturados
- [ ] Configurar monitoramento

## 📈 Possíveis Melhorias Futuras

### Funcionalidades
- [ ] Sistema de autenticação (Identity)
- [ ] Relatórios e dashboards avançados
- [ ] Upload de arquivos (documentos, fotos)
- [ ] Sistema de notificações
- [ ] API REST para integração
- [ ] Exportação para Excel/PDF

### Técnicas
- [ ] Implementar CQRS
- [ ] Adicionar cache (Redis)
- [ ] Background jobs (Hangfire)
- [ ] Testes automatizados
- [ ] Docker/containerização
- [ ] CI/CD pipeline

## 🤝 Contribuições

Este projeto foi desenvolvido como um exemplo educacional de implementação de relacionamentos N:N em ASP.NET Core MVC. Contribuições são bem-vindas!

### Como Contribuir
1. Fork o projeto
2. Crie uma branch para sua feature (`git checkout -b feature/AmazingFeature`)
3. Commit suas mudanças (`git commit -m 'Add some AmazingFeature'`)
4. Push para a branch (`git push origin feature/AmazingFeature`)
5. Abra um Pull Request

## 📄 Licença

Este projeto é distribuído sob a licença MIT. Veja `LICENSE` para mais informações.

## 📞 Contato

Para dúvidas ou sugestões sobre este projeto, abra uma issue no repositório.

---

**Plataforma EAD** - Sistema de gestão educacional desenvolvido com ❤️ usando ASP.NET Core MVC
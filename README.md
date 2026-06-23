# Agentic CRM

An AI-powered CRM that deploys three Claude agents in parallel to generate personalized sales outreach, marketing campaigns, and customer onboarding plans — in seconds.

Built with **ASP.NET Core 8 Blazor Server** and the **Anthropic Claude API**.

![.NET 8](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet) ![Blazor](https://img.shields.io/badge/Blazor-Server-512BD4?logo=blazor) ![Claude](https://img.shields.io/badge/Claude-claude--opus--4--8-D97706)

---

## What it does

Select a lead and three AI agents spin up concurrently:

| Agent | Output |
|---|---|
| **Sales (SDR)** | Lead qualification score, strategic thinking, personalized email draft |
| **Marketing** | Campaign headline, multi-channel strategy, campaign copy |
| **Customer Success** | Onboarding roadmap, numbered checklist, welcome letter |

All three run in parallel via `Task.WhenAll()`. A real-time log panel streams each agent's reasoning, output, and errors as they execute.

---

## Tech stack

- **ASP.NET Core 8** — Blazor Server with interactive rendering
- **C#** — nullable reference types enabled
- **Anthropic Claude API** — `claude-opus-4-8` model, per-agent temperature tuning
- **Tailwind CSS 4** — responsive, mobile-first UI
- **Bootstrap Icons** — iconography

No database — leads and agent logs are held in-memory with thread-safe collections.

---

## Getting started

### Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) or later
- An [Anthropic API key](https://console.anthropic.com/)

### Setup

1. **Clone the repo**

   ```bash
   git clone https://github.com/virtual-intelligence-kinetic-interface/agentic-crm.git
   cd agentic-crm
   ```

2. **Configure your API key**

   Copy the example env file and add your key:

   ```bash
   cp .env.example .env
   ```

   Then edit `.env`:

   ```
   ANTHROPIC_API_KEY=your_key_here
   ```

   Or set it directly in `appsettings.json`:

   ```json
   {
     "Anthropic": {
       "ApiKey": "your_key_here"
     }
   }
   ```

3. **Run**

   ```bash
   dotnet run
   ```

   Open `https://localhost:56437` in your browser.

> **No API key?** The app falls back to a simulation mode with realistic mock outputs so you can explore the UI without credentials.

---

## Project structure

```
agentic-crm/
├── Components/
│   └── Pages/
│       └── Home.razor          # Main dashboard — lead list, kanban, agent outputs
├── Services/
│   ├── AnthropicService.cs     # Claude API client (HTTP, JSON parsing, error handling)
│   └── CrmService.cs           # In-memory store with event-driven UI updates
├── Models/
│   ├── Lead.cs                 # Lead entity with per-agent output fields
│   └── AgentLog.cs             # Execution trace for the log panel
├── Program.cs                  # DI setup, middleware, service registration
└── .env.example                # API key template
```

---

## Features

- **Parallel agent orchestration** — all three agents run simultaneously, not sequentially
- **Live log streaming** — terminal-style panel shows timestamps, agent ID, reasoning, and errors in real time
- **Kanban pipeline** — drag leads across stages: New → Qualified → Nurturing → Proposal → Won → Challenged
- **Lead management** — create, search, filter, and delete leads via the prospect directory
- **Copyable outputs** — one-click copy for email drafts, campaign copy, and welcome letters
- **Graceful degradation** — API failures automatically trigger demo mode; no crashes

---

## Architecture notes

- `CrmService` is a singleton holding all leads and logs; it fires `OnChange` events that trigger Blazor re-renders without polling
- `AnthropicService` is also a singleton wrapping a shared `HttpClient`, with per-request temperature and max-token settings per agent role
- Agents return structured JSON; the client validates and falls back to defaults on parse failure
- Locks around collections prevent race conditions under concurrent agent execution

---

## Demo data

Three realistic seed leads ship with the app:

- **Sarah Jenkins** — VP Engineering, CloudPulse Inc. (Cloud Infrastructure)
- **Marcus Chen** — Director of Operations, FinGo Payments (FinTech)
- **Elena Rostova** — Co-founder/COO, GreenTrace AgTech (Biotech)

Each includes a budget, tags, notes, and pre-generated sample agent outputs so the UI is fully explorable on first load.

---

## License

MIT

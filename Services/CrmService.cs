using AgenticCrm.Models;

namespace AgenticCrm.Services;

public class CrmService
{
    private readonly List<Lead> _leads;
    private readonly List<AgentLog> _logs;
    private readonly object _lock = new();

    public event Action? OnChange;

    public CrmService()
    {
        _leads = new List<Lead>
        {
            new()
            {
                Id = "lead-1",
                Name = "Sarah Jenkins",
                Company = "CloudPulse Inc.",
                Title = "VP of Engineering",
                Email = "sarah.jenkins@cloudpulse.io",
                Phone = "+1 (555) 438-2910",
                Status = "New",
                Source = "Webinar Attendee",
                Budget = "$45,000 / year",
                CompanySize = "150-250 employees",
                Industry = "Cloud Infrastructure",
                CoreProductInterest = "Real-time scaling and multi-region anomaly alerts",
                Tags = new() { "High Value", "AWS Ecosystem", "Quick Decision Maker" },
                LastActionDate = DateTime.Parse("2026-06-19T08:30:00Z").ToUniversalTime(),
                Notes = "Attended webinar on agentic workflows. Indicated high friction when scaling across cluster deployments in Frankfurt.",
                SalesScore = 82,
                SalesThought = "Strong background, VP level role, good budget match. Needs technical demonstration of cluster monitoring.",
                SalesEmailDraft = "Subject: Streamlining cluster scalability anomaly monitoring at CloudPulse\n\nHi Sarah,\n\nThanks for attending our Webinar last Tuesday. I noticed you called out Frankfurt multi-zone lag as a current headache. Let's schedule a 15-minute sync to show you how our automation handles scaling friction without false alerted margins.\n\nWarm regards,\nSales Agent",
                MarketingThought = "Cloud infrastructure lead is highly receptive to optimization and capacity automation strategies.",
                MarketingPitch = "Deploy parallel SDR pipeline metrics to scale multi-region environments cleanly.",
                SuccessThought = "Post-demo checklist will require specialized AWS integration guides and quick latency testbeds.",
                SuccessChecklist = new() { "Provide Terraform multi-region lag dashboard blueprint", "Create Slack integration threshold guide", "Schedule 30-min architecture design review" }
            },
            new()
            {
                Id = "lead-2",
                Name = "Marcus Chen",
                Company = "FinGo Payments",
                Title = "Director of Marketing Operations",
                Email = "m.chen@fingopayments.com",
                Phone = "+1 (212) 887-2345",
                Status = "Qualified",
                Source = "Inbound LiveChat",
                Budget = "$120,000 / year",
                CompanySize = "1,000+ employees",
                Industry = "FinTech / Payment Services",
                CoreProductInterest = "SOC-2 compliant CRM drip triggers and automated lead sync",
                Tags = new() { "Enterprise", "Compliance Focus", "Competitor Renewal" },
                LastActionDate = DateTime.Parse("2026-06-18T14:15:00Z").ToUniversalTime(),
                Notes = "Currently on competitor package. Looking for a tighter SOC-2 audit log and rapid custom webhooks support.",
                SalesScore = 94,
                SalesThought = "Elite lead. Budget is well above general platform hurdles. Marcus is anxious about SOC-2 and competitor migration fatigue.",
                SalesEmailDraft = "Subject: Compliant operations audit workflow migration for FinGo\n\nHi Marcus,\n\nI understand FinGo is approaching renewal on SalesForce next quarter. Our system provides direct migration tooling plus persistent compliance logging natively. Let's arrange a deep dive with our architect.\n\nBest,\nSDR Platform Leader"
            },
            new()
            {
                Id = "lead-3",
                Name = "Elena Rostova",
                Company = "GreenTrace AgTech",
                Title = "Co-founder & COO",
                Email = "elena@greentrace.farm",
                Phone = "+33 607 88 12 34",
                Status = "Nurturing",
                Source = "Referral",
                Budget = "$8,500 / year",
                CompanySize = "10-50 employees",
                Industry = "Biotech / AgriTech",
                CoreProductInterest = "Low-code dashboard for tracking plant metrics offline",
                Tags = new() { "SMB", "AgriTech", "Offline Priority" },
                LastActionDate = DateTime.Parse("2026-06-15T11:00:00Z").ToUniversalTime(),
                Notes = "Looking for simple client-side persistence and beautiful dashboards to run on remote agricultural devices with shaky connections."
            }
        };

        _logs = new List<AgentLog>
        {
            new() { Id = "log-seed-1", AgentId = "sales", LeadId = "lead-1", LeadName = "Sarah Jenkins", Timestamp = DateTime.Parse("2026-06-19T08:35:00Z").ToUniversalTime(), Type = "info", Message = "Sales agent assigned to assess firmware & firmographic alignment for CloudPulse." },
            new() { Id = "log-seed-2", AgentId = "sales", LeadId = "lead-1", LeadName = "Sarah Jenkins", Timestamp = DateTime.Parse("2026-06-19T08:36:12Z").ToUniversalTime(), Type = "reasoning", Message = "Analyzing company size (150-250) and budget ($45,000/yr). High buyer authority confirmed for VP of Engineering." },
            new() { Id = "log-seed-3", AgentId = "sales", LeadId = "lead-1", LeadName = "Sarah Jenkins", Timestamp = DateTime.Parse("2026-06-19T08:36:55Z").ToUniversalTime(), Type = "output", Message = "Generated tailored Salesscore: 82. Drafted specialized scaling email outreach.", Payload = "Subject: Streamlining cluster scalability... [Draft Attached]" }
        };
    }

    public IReadOnlyList<Lead> Leads { get { lock (_lock) return _leads.ToList(); } }
    public IReadOnlyList<AgentLog> Logs { get { lock (_lock) return _logs.ToList(); } }

    public Lead? GetLead(string id) { lock (_lock) return _leads.FirstOrDefault(l => l.Id == id); }

    public void AddLog(string agentId, string leadId, string leadName, string type, string message, string? payload = null)
    {
        var log = new AgentLog
        {
            Id = $"log-{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}-{Guid.NewGuid().ToString("N")[..8]}",
            AgentId = agentId,
            LeadId = leadId,
            LeadName = leadName,
            Timestamp = DateTime.UtcNow,
            Type = type,
            Message = message,
            Payload = payload
        };
        lock (_lock)
        {
            _logs.Insert(0, log);
            if (_logs.Count > 200) _logs.RemoveAt(_logs.Count - 1);
        }
        OnChange?.Invoke();
    }

    public Lead AddLead(Lead lead)
    {
        lock (_lock) { _leads.Add(lead); }
        OnChange?.Invoke();
        return lead;
    }

    public Lead? UpdateLead(string id, Action<Lead> update)
    {
        Lead? lead;
        lock (_lock)
        {
            lead = _leads.FirstOrDefault(l => l.Id == id);
            if (lead is not null) update(lead);
        }
        if (lead is not null) OnChange?.Invoke();
        return lead;
    }

    public bool DeleteLead(string id)
    {
        bool removed;
        lock (_lock) { removed = _leads.RemoveAll(l => l.Id == id) > 0; }
        if (removed) OnChange?.Invoke();
        return removed;
    }

    public Lead? ResetLead(string id) => UpdateLead(id, l =>
    {
        l.SalesScore = null; l.SalesEmailDraft = null; l.SalesThought = null;
        l.MarketingPitch = null; l.MarketingCampaign = null; l.MarketingThought = null;
        l.SuccessChecklist = null; l.SuccessResponse = null; l.SuccessThought = null;
        l.LastActionDate = DateTime.UtcNow;
    });
}

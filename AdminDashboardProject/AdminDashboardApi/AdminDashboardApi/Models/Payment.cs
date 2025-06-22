namespace AdminDashboardApi.Models;

public class Payment
{
	public int Id { get; set; }
	public int ClientId { get; set; }
	public decimal Amount { get; set; }
	public DateTime Date { get; set; }
	public Client Client { get; set; }
}

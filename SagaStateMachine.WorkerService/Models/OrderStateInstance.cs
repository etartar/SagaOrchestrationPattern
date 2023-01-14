using Automatonymous;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Text;

namespace SagaStateMachine.WorkerService.Models
{
    public class OrderStateInstance : SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }
        public string CurrenctState { get; set; }
        public string BuyerId { get; set; }
        public int OrderId { get; set; }
        public string CardName { get; set; }
        public string CardNumber { get; set; }
        public string Expiration { get; set; }
        public string CVV { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPrice { get; set; }
        public DateTime CreatedDate { get; set; }

        public override string ToString()
        {
            PropertyInfo[] properties = GetType().GetProperties();

            StringBuilder sb = new StringBuilder();

            properties.ToList().ForEach(p =>
            {
                object? value = p.GetValue(this, null);
                sb.AppendLine($"{p.Name}:{value}");
            });

            sb.AppendLine("-----------------------");

            return sb.ToString();
        }
    }
}

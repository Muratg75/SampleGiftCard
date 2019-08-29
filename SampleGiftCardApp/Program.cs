using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleGiftCardApp
{
    class Program
    {
        //init data

        public static Dictionary<string, GiftCard> Gifs;

        public static Dictionary<string, RPAccount> Accounts;
        

        static void Main(string[] args)
        {



            //init data

            Gifs = new Dictionary<string, GiftCard>();
            Accounts = new Dictionary<string, RPAccount>();

            RPAccount r0 = new RPAccount() { Name = "Root", ActualAmount = 0 };

            //Create Gift Crads
            for (int i = 1; i < 10; i++)
            {
                string sKey = "AAA" + i;
                GiftCard g = new GiftCard();
                g.Code = sKey;
                g.Amount = i * 50;
                Gifs.Add(sKey, g);

                GiftTrans t0 = new GiftTrans { TransDate = DateTime.Now, RPAccountTo = r0, GiftCard = Gifs["AAA" + i]};
                t0.Create();

            }

            //Create Accounts
            RPAccount r1 = new RPAccount() { Name = "Murat", ActualAmount = 0 };
            RPAccount r2 = new RPAccount() { Name = "Test1", ActualAmount = 0 };
            RPAccount r3 = new RPAccount() { Name = "Test2", ActualAmount = 0 };

            //
            GiftTrans t1 = new GiftTrans { TransDate = DateTime.Now, GiftCard= Gifs["AAA1"] };
            t1.Transfer(r0,r1);

            GiftTrans t2 = new GiftTrans { TransDate = DateTime.Now, GiftCard = Gifs["AAA2"] };
            t1.Transfer(r0, r2);

            GiftTrans t3 = new GiftTrans { TransDate = DateTime.Now, GiftCard = Gifs["AAA5"] };
            t1.Transfer(r0, r3);

        }

    }

    class RPAccount
    {
        public string Name { get; set; }
        public decimal ActualAmount { get; set; }
    }

    class GiftCard
    {
        public string Code { get; set; }
        public decimal Amount { get; set; }
    }

    class GiftTrans
    {
        public static Dictionary<string, GiftTrans> Trans = new Dictionary<string, GiftTrans>();

        public DateTime TransDate { get; set; }

        public RPAccount RPAccountFrom { get; set; }
        public RPAccount RPAccountTo { get; set; }
        public GiftCard GiftCard { get; set; }

        public GiftStatus Status { get; set; }  
        public decimal Amount { get; set; }

        public void Create()
        {
            this.Amount=this.GiftCard.Amount;
            this.RPAccountTo.ActualAmount = + this.GiftCard.Amount;
            this.Status = GiftStatus.Created;
            Trans.Add(Guid.NewGuid().ToString(), this);
        }

        public void Transfer(RPAccount from , RPAccount to)
        {
            this.Amount = this.GiftCard.Amount;
            from.ActualAmount -=this.GiftCard.Amount;
            to.ActualAmount  += this.GiftCard.Amount;
            this.RPAccountFrom = from;
            this.RPAccountTo = to;
            this.Status = GiftStatus.Transfered;
            Trans.Add(Guid.NewGuid().ToString(), this);
        }
    }

    public enum GiftStatus
    {
        Created=1,
        Transfered=2,
        Sold=3,
        Cancel=4,
        Deleted=5
    }
}

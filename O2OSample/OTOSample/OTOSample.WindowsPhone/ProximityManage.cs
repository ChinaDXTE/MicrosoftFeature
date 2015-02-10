using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Proximity;

namespace OTOSample
{
    public class ProximityManage
    {
        #region field

        private ProximityDevice proximityDevice;
        private long publishedMessageId = -1;
        private long subscribedMessageId = -1;
        #endregion

        public ProximityManage()
        {
            this.proximityDevice = ProximityDevice.GetDefault();
        }

        /// <summary>
        /// stop proximity
        /// </summary>
        public void StopProDevice()
        {
            if (this.proximityDevice != null)
            {
                this.proximityDevice.StopPublishingMessage(this.publishedMessageId);
                this.proximityDevice.StopSubscribingForMessage(this.subscribedMessageId);
                this.publishedMessageId = -1;
                this.subscribedMessageId = -1;
            }
        }

        /// <summary>
        /// publish message
        /// </summary>
        /// <param name="des"></param>
        /// <param name="pay"></param>
        public void PublishMessage(string des,string pay)
        {
            if (this.publishedMessageId == -1)
            {
                String message = des + "-" + pay;
                if (message.Length > 0)
                {
                    this.publishedMessageId = this.proximityDevice.PublishMessage("Windows.SampleMessageType", message);
                }
            }
        }

        /// <summary>
        /// sub message
        /// </summary>
        public void SubscribeForMessage()
        {
            if (this.subscribedMessageId == -1)
            {
                this.subscribedMessageId = this.proximityDevice.SubscribeForMessage("Windows.SampleMessageType", messageReceived);
            }
        }

        /// <summary>
        /// message received
        /// </summary>
        /// <param name="proximityDevice"></param>
        /// <param name="message"></param>
        private void messageReceived(ProximityDevice proximityDevice, ProximityMessage message)
        {
            string data = message.DataAsString;
            string [] datas = data.Split('-');
            string des = datas[0].ToString();
            double pay = double.Parse(data[1].ToString());
            WalletManage walletManage = new WalletManage();
            walletManage.Transaction(des,pay);
        }
    }
}

using Feature;
using Feature.DataModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Wallet;
using Windows.Storage;

namespace OTOSample
{
    public class WalletManage
    {
        #region field

        private const string conCardID = "OTOSample_MyCard";
        private const string conCardMoney = "OTOSample_Card_Money";
        private WalletItemStore wallet;
        #endregion

        #region init
        public WalletManage()
        {
            this.initWallet();
        }

        private async void initWallet()
        {
            this.wallet = await WalletManager.RequestStoreAsync();
        }
        #endregion

        /// <summary>
        /// create Wallet
        /// </summary>
        public async void CreateWallet()
        {
            try
            {
                // Create the membership card.
                WalletItem card = new WalletItem(WalletItemKind.MembershipCard, "My Card");
                // Set colors, to give the card our distinct branding.
                card.BodyColor = Windows.UI.Colors.Brown;
                card.BodyFontColor = Windows.UI.Colors.White;
                card.HeaderColor = Windows.UI.Colors.SaddleBrown;
                card.HeaderFontColor = Windows.UI.Colors.White;
                // Set basic properties.
                card.IssuerDisplayName = "Bank";
                // Set some images.
                card.Logo336x336 = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///assets/wallet/coffee336x336.png"));
                card.Logo99x99 = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///assets/wallet/coffee99x99.png"));
                card.Logo159x159 = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///assets/wallet/coffee159x159.png"));
                card.HeaderBackgroundImage = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///assets/wallet/header640x130.png"));
                // Set the loyalty card points and show them on the detailed view of card and in the list view.
                WalletItemCustomProperty prop = new WalletItemCustomProperty("Money", "1000000");
                prop.DetailViewPosition = WalletDetailViewPosition.FooterField1;
                prop.SummaryViewPosition = WalletSummaryViewPosition.Field1;
                card.DisplayProperties[conCardMoney] = prop;
                // Show the branch.
                prop = new WalletItemCustomProperty("Branch", "Bank on 5th");
                prop.DetailViewPosition = WalletDetailViewPosition.HeaderField1;
                card.DisplayProperties["BranchId"] = prop;
                // Add the customer account number.
                prop = new WalletItemCustomProperty("Account Number", "2014************3697");
                prop.DetailViewPosition = WalletDetailViewPosition.FooterField2;
                // We don't want this field entity extracted as it will be interpreted as a phone number.
                prop.AutoDetectLinks = false;
                card.DisplayProperties["AcctId"] = prop;
                // Encode the user's account number as a Qr Code to be used in the store.
                card.Barcode = new WalletBarcode(WalletBarcodeSymbology.Qr, "20141212025641693697");
                // Add a promotional message to the card.
                card.DisplayMessage = "Tap here for the fast payment";
                card.IsDisplayMessageLaunchable = true;
                await wallet.AddAsync(conCardID, card);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// show wallet
        /// </summary>
        /// <returns></returns>
        public async Task ShowWalletItemAsync()
        {
            try
            {
                WalletItem walletItem = await wallet.GetWalletItemAsync(conCardID);
                if (walletItem != null)
                {
                    await this.wallet.ShowAsync(walletItem.Id);
                }
                else
                {
                    await this.wallet.ShowAsync();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// transaction
        /// </summary>
        /// <param name="des">description</param>
        /// <param name="pay">paymet</param>
        public async void Transaction(string des,double pay)
        {
            try
            {
                SampleItem itemData = null;
                bool isFind = false;
                foreach (GroupInfoList group in Global.Current.SampleItems.Data)
                {
                    foreach (SampleItem item in group.ItemContent)
                    {
                        if (item.Title == des)
                        {
                            itemData = item;
                            itemData.Price = "￥" + pay.ToString();
                            des = des + "-付款";
                            isFind = true;
                            break;
                        }
                    }
                    if (isFind)
                        break;
                }
                if (itemData != null)
                {
                    Global.Current.Serialization.SaveOrder(itemData);
                }
                WalletItem walletItem = await this.wallet.GetWalletItemAsync(conCardID);
                if (walletItem != null && walletItem.DisplayProperties.ContainsKey(conCardMoney))
                {
                    string money = walletItem.DisplayProperties[conCardMoney].Value;
                    string updateMoney = (double.Parse(money) - pay).ToString();
                    walletItem.DisplayProperties[conCardMoney].Value = updateMoney;
                    await this.wallet.UpdateAsync(walletItem);
                    WalletTransaction walletTransaction = new WalletTransaction();
                    walletTransaction.Description = des;
                    walletTransaction.DisplayAmount = "￥" + pay.ToString();
                    walletTransaction.TransactionDate = DateTime.Now;
                    walletTransaction.IgnoreTimeOfDay = false;
                    walletTransaction.DisplayLocation = "Bank on 5th";
                    // Add the transaction to the TransactionHistory of this item.
                    walletItem.TransactionHistory.Add("transaction" + DateTime.Now.Ticks.ToString(), walletTransaction);
                    // Update the item in Wallet.
                    await this.wallet.UpdateAsync(walletItem);
                    Global.Current.Notifications.CreateToastNotifier("提示","付款成功");
                    Global.Current.Notifications.CreateTileWide310x150Text09("付款成功","恭喜您付款成功");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// delete wallet
        /// </summary>
        public async void DeleteWallet()
        {
            try
            {
                WalletItem walletItem = await this.wallet.GetWalletItemAsync(conCardID);
                await this.wallet.DeleteAsync(walletItem.Id);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class IAPManager : MonoSingleton<IAPManager>, IStoreListener
{
    [Serializable]
    public struct ProductModel
    {
        public string ID;
        public ProductType EPurchase;
    }
    [SerializeField] ProductModel[] _models;

    #region :   Protected

    protected override void OnInitialize()
    {
        base.OnInitialize();

        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        for (int i = 0; i < _models.Length; i++)
        {
            string productID = _models[i].ID;

            builder.AddProduct(productID, _models[i].EPurchase, new IDs
            {
                {productID, GooglePlay.Name },
                {productID, AppleAppStore.Name }
            });
        }

        UnityPurchasing.Initialize(this, builder);
    }

    #endregion

    #region :   Interface

    void IStoreListener.OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {

    }

    void IStoreListener.OnInitializeFailed(InitializationFailureReason error)
    {

    }

    void IStoreListener.OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {

    }

    PurchaseProcessingResult IStoreListener.ProcessPurchase(PurchaseEventArgs purchaseEvent)
    {
        return PurchaseProcessingResult.Complete;
    }

    #endregion
}

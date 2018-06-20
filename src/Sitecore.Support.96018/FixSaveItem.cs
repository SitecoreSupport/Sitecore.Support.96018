namespace Sitecore.Support
{
  using Sitecore.Events.Hooks;
  using Sitecore.SecurityModel;
  using Sitecore.Data;
  using Sitecore.Configuration;
  using Sitecore.Data.Items;
  using System;
  using Sitecore.Diagnostics;
  using Sitecore.Data.Fields;

  public class FixSaveItem : IHook
  {
    private string coreDbName;
    public FixSaveItem(string coreDb)
    {
      Assert.ArgumentNotNullOrEmpty(coreDb, "coreDb");
      this.coreDbName = coreDb;
    }
    public void Initialize()
    {
      Database coreDb = Factory.GetDatabase(coreDbName);
      using (new SecurityDisabler())
      {
        try
        {
          Item saveButtonItem = coreDb.GetItem("{12FF26DE-2BBC-4E60-A43A-735DF841E3CA}");          
          Field keyCodeField = saveButtonItem.Fields[new ID("{CCD6BDE9-0A7A-4DA3-B0ED-D3C0F65379FE}")];
          if (keyCodeField.Value != "")
          {
            using (new EditContext(saveButtonItem))
            {
              keyCodeField.Value = "";
            }
          }
        }
        catch (NullReferenceException e)
        {
          Log.Error("NullReferenceException is catched in Sitecore.Support.96018 patch. The error occurred while editing Save button item: ", e, this);
        }
      }
    }
  }
}
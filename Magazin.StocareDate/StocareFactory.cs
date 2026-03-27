namespace Magazin.StocareDate
{
    public static class StocareFactory
    {
        public static IStocareData GetStocare()
        {
            return new AdministrareProduseFisierText();
        }
    }
}
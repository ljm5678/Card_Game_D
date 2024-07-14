using UnityEngine;

public static class CardFactory
{
    public static CardItem CreateCard(ref GameObject parent, int id)
    {
        CardItem cardItem = null;
        // Initialize card type or other properties based on cardType parameter
        switch (id)
        {
            case 0:
                 cardItem = parent.AddComponent<Card_0>();
                break;
            case 1:
                cardItem = parent.AddComponent<Card_1>();
                break;
            case 2:
                cardItem = parent.AddComponent<Card_2>();
                break;
            case 3:
                cardItem = parent.AddComponent<Card_3>();
                break;
            case 4:
                cardItem = parent.AddComponent<Card_4>();
                break;
            case 5:
                cardItem = parent.AddComponent<Card_5>();
                break;
            case 6:
                cardItem = parent.AddComponent<Card_6>();
                break;
            default:
                cardItem = parent.AddComponent<Card_0>();
                break;

        }



        cardItem.Initialize(id);
        return cardItem;
    }
}

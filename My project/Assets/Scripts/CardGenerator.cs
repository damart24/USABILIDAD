using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class CardGenerator : MonoBehaviour
{
    string fileName = "niveles-cartas.csv";
    //string filePath = "niveles-cartas.csv";
    //Prefab de la carta que se va a generar
    [SerializeField]
    private GameObject cardPrefab_;
    //Lista de sprites aleatorios que se van a cambiar
    [SerializeField]
    private Sprite[] sprites_;

    private int numCartas = 0;
    List<Carta> cartas = new List<Carta>();
    List<Carta> cartasFijas = new List<Carta>();
    private void Start()
    {
        string filePath = Path.Combine("Assets", "CSV", fileName);
        string[] lines = File.ReadAllLines(filePath);
        numCartas = 0;

        foreach (string line in lines)
        {
            string[] values = line.Split(',');

            Carta carta = new Carta();
            carta.Tema = values[0];
            carta.Nombre = values[1];
            carta.Personaje = values[2];
            carta.Pregunta = values[3];
            carta.Condicion = values[4];
            carta.Probabilidad = int.TryParse(values[5], out int probabilidad) ? probabilidad : 0;
            carta.SobrescribirSi = values[6];
            carta.SiDinero = int.TryParse(values[7], out int siDinero) ? siDinero : 0;
            carta.SiGente = int.TryParse(values[8], out int siGente) ? siGente : 0;
            carta.SiFlora = int.TryParse(values[9], out int siFlora) ? siFlora : 0;
            carta.SiFauna = int.TryParse(values[10], out int siFauna) ? siFauna : 0;
            carta.SiAire = int.TryParse(values[11], out int siAire) ? siAire : 0;
            carta.ExtrasSi = values[12];
            carta.SobrescribeNo = values[13]; 
            carta.NoDinero = int.TryParse(values[14], out int noDinero) ? noDinero : 0;
            carta.NoGente = int.TryParse(values[15], out int noGente) ? noGente : 0;
            carta.NoFlora = int.TryParse(values[16], out int noFlora) ? noFlora : 0;
            carta.NoFauna = int.TryParse(values[17], out int noFauna) ? noFauna : 0;
            carta.NoAire = int.TryParse(values[18], out int noAire) ? noAire : 0;
            carta.ExtrasNo = values[19];
            carta.TextoExplicativo = values[20];
            carta.CardId = numCartas;
            carta.Usada = false;

            numCartas++;
            cartas.Add(carta);
        }
        cartasFijas = cartas.FindAll(carta => carta.Condicion == "Evento fijo");
        GameManager.Instance.cartasPorPartida = GenerateFixedCardsSelection();
    }

    //Instancia la nueva carta que estará detrás del mazo
    //Le pone como primer hijo y cambia su sprite por un sprite aleatorio
    public void InstantiateCard()
    {
        GameManager gameManager = GameManager.Instance;
        int num = gameManager.cardsCount;

        if (num < gameManager.cartasPorPartida.Count)
        {
            GameObject newCard = Instantiate(cardPrefab_, transform, false);
            newCard.transform.SetAsFirstSibling();

            Type cardType = null;
            try
            {
                cardType = gameManager.cartasPorPartida[num].GetType();
            }
            catch
            {

            }

            if (cardType == null)
            {
                Carta carta = SelectRandomCard();
                gameManager.cartasPorPartida[gameManager.cardsCount] = carta;
            }

            Carta playerCard = newCard.GetComponent<Carta>();

            playerCard.Tema = gameManager.cartasPorPartida[num].Tema;
            playerCard.Nombre = gameManager.cartasPorPartida[num].Nombre;
            playerCard.Personaje = gameManager.cartasPorPartida[num].Personaje;
            playerCard.Pregunta = gameManager.cartasPorPartida[num].Pregunta;
            playerCard.Condicion = gameManager.cartasPorPartida[num].Condicion;
            playerCard.Probabilidad = gameManager.cartasPorPartida[num].Probabilidad;
            playerCard.SobrescribirSi = gameManager.cartasPorPartida[num].SobrescribirSi;
            playerCard.SiDinero = gameManager.cartasPorPartida[num].SiDinero;
            playerCard.SiGente = gameManager.cartasPorPartida[num].SiGente;
            playerCard.SiFlora = gameManager.cartasPorPartida[num].SiFlora;
            playerCard.SiFauna = gameManager.cartasPorPartida[num].SiFauna;
            playerCard.SiAire = gameManager.cartasPorPartida[num].SiAire;
            playerCard.ExtrasSi = gameManager.cartasPorPartida[num].ExtrasSi;
            playerCard.SobrescribeNo = gameManager.cartasPorPartida[num].SobrescribeNo;
            playerCard.NoDinero = gameManager.cartasPorPartida[num].NoDinero;
            playerCard.NoGente = gameManager.cartasPorPartida[num].NoGente;
            playerCard.NoFlora = gameManager.cartasPorPartida[num].NoFlora;
            playerCard.NoFauna = gameManager.cartasPorPartida[num].NoFauna;
            playerCard.NoAire = gameManager.cartasPorPartida[num].NoAire;
            playerCard.ExtrasNo = gameManager.cartasPorPartida[num].ExtrasNo;
            playerCard.TextoExplicativo = gameManager.cartasPorPartida[num].TextoExplicativo;

            if (playerCard.SobrescribeNo == "")
                playerCard.SobrescribeNo = "No";

            if (playerCard.SobrescribirSi == "")
                playerCard.SobrescribirSi = "Si";

            if (gameManager.cartasPorPartida[num].Personaje == "Faustino el agricultor")
            {
                newCard.GetComponent<Image>().sprite = sprites_[0];
            }
            else if (gameManager.cartasPorPartida[num].Personaje == "Toni el activista")
            {
                newCard.GetComponent<Image>().sprite = sprites_[1];
            }
            else if (gameManager.cartasPorPartida[num].Personaje == "Paqui Jefa de Medio Ambiente")
            {
                newCard.GetComponent<Image>().sprite = sprites_[2];
            }
            else
            {
                newCard.GetComponent<Image>().sprite = sprites_[3];
            }
            cartas[gameManager.cartasPorPartida[gameManager.cardsCount].CardId].Usada = true;
            gameManager.cardsCount++;
        }
        else
            GameManager.Instance.gameWon = true;
    }

    public List<Carta> GenerateFixedCardsSelection()
    {
        int[] intCardsSelection = new int[7];
        intCardsSelection[0] = SelectRandomFixedCard("Agricultura");
        intCardsSelection[1] = SelectRandomFixedCard("Deforestación"); ;
        intCardsSelection[2] = SelectRandomFixedCard("Energía eólica"); ;
        intCardsSelection[3] = SelectRandomFixedCard("Fabrica/Economia");
        intCardsSelection[4] = SelectRandomFixedCard("Ganaderia");
        intCardsSelection[5] = SelectRandomFixedCard("Energía Solar");
        intCardsSelection[6] = SelectRandomFixedCard("Prevención de incendios");

        System.Random rng = new System.Random();
        int n = intCardsSelection.Length;

        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            int value = intCardsSelection[k];
            intCardsSelection[k] = intCardsSelection[n];
            intCardsSelection[n] = value;
        }

        List<Carta> fixedCardsSelection = new List<Carta>(20);
        for (int i = 0; i < 20; i++)
        {
            fixedCardsSelection.Add(null);
        }

        // Lista de números del 0 al 20
        List<int> numberList = new List<int>(new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19});

        // Usar la clase Random para seleccionar cinco números aleatorios
        List<int> randomNumberList = new List<int>();

        for (int i = 0; i < 7; i++)
        {
            int indiceAleatorio = UnityEngine.Random.Range(0, numberList.Count);
            int numeroAleatorio = numberList[indiceAleatorio];
            randomNumberList.Add(numeroAleatorio);

            // Remover el número seleccionado para evitar repeticiones
            numberList.RemoveAt(indiceAleatorio);
        }

        for (int i = 0; i < randomNumberList.Count; i++)
        {
            fixedCardsSelection[randomNumberList[i]] = cartas[intCardsSelection[i]];
        }

        return fixedCardsSelection;
    }

    int SelectRandomFixedCard(string condicion)
    {
        List<Carta> cartasFiltradas = cartasFijas.FindAll(carta => carta.Tema == condicion);

        int indiceAleatorio = UnityEngine.Random.Range(0, cartasFiltradas.Count);
        return cartasFiltradas[indiceAleatorio].CardId;
    }

    Carta SelectRandomCard()
    {

        List<Carta> cartasNoFijas = cartas.FindAll(carta => carta.Condicion != "Evento fijo");

        int indiceAleatorio = -1;
        do
        {
            indiceAleatorio = UnityEngine.Random.Range(0, cartasNoFijas.Count);
        } while (cartas[indiceAleatorio].Usada || (cartas[indiceAleatorio].Condicion != "Evento fijo" && !GameManager.Instance.conditions.Contains(cartas[indiceAleatorio].Condicion)));

        return cartas[cartasNoFijas[indiceAleatorio].CardId];
    }
}

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

            Carta carta = new Carta
            {
                Tema = values[0],
                Nombre = values[1],
                Personaje = values[2],
                Pregunta = values[3],
                Condicion = values[4],
                // Utilizando int.TryParse para manejar la conversión a entero de Probabilidad
                Probabilidad = int.TryParse(values[5], out int probabilidad) ? probabilidad : 0,

                SobrescribirSi = values[6],

                // Utilizando int.TryParse para manejar la conversión a entero de SiDinero
                SiDinero = int.TryParse(values[7], out int siDinero) ? siDinero : 0,

                // Utilizando int.TryParse para manejar la conversión a entero de SiGente
                SiGente = int.TryParse(values[8], out int siGente) ? siGente : 0,

                // Utilizando int.TryParse para manejar la conversión a entero de SiFlora
                SiFlora = int.TryParse(values[9], out int siFlora) ? siFlora : 0,

                // Utilizando int.TryParse para manejar la conversión a entero de SiFauna
                SiFauna = int.TryParse(values[10], out int siFauna) ? siFauna : 0,

                // Utilizando int.TryParse para manejar la conversión a entero de SiAire
                SiAire = int.TryParse(values[11], out int siAire) ? siAire : 0,

                ExtrasSi = values[12],
                SobrescribeNo = values[13],

                // Utilizando int.TryParse para manejar la conversión a entero de NoDinero
                NoDinero = int.TryParse(values[14], out int noDinero) ? noDinero : 0,

                // Utilizando int.TryParse para manejar la conversión a entero de NoGente
                NoGente = int.TryParse(values[15], out int noGente) ? noGente : 0,

                // Utilizando int.TryParse para manejar la conversión a entero de NoFlora
                NoFlora = int.TryParse(values[16], out int noFlora) ? noFlora : 0,

                // Utilizando int.TryParse para manejar la conversión a entero de NoFauna
                NoFauna = int.TryParse(values[17], out int noFauna) ? noFauna : 0,

                // Utilizando int.TryParse para manejar la conversión a entero de NoAire
                NoAire = int.TryParse(values[18], out int noAire) ? noAire : 0,

                ExtrasNo = values[19],
                TextoExplicativo = values[20],
                // Utilizando int.TryParse para manejar la conversión a entero de NoAire
                CardId = numCartas,
                Usada = false,
            };
            numCartas++;
            cartas.Add(carta);
        }
        cartasFijas = cartas.FindAll(carta => carta.Condicion == "Evento fijo");
      /*  GameManager.Instance.cartasPorPartida = */GenerateFixedCardsSelection();
    }

    //Instancia la nueva carta que estará detrás del mazo
    //Le pone como primer hijo y cambia su sprite por un sprite aleatorio
    public void InstantiateCard()
    {
        GameObject newCard = Instantiate(cardPrefab_, transform, false);
        newCard.transform.SetAsFirstSibling();
        int num = UnityEngine.Random.Range(0, cartas.Count);

        Carta playerCard = newCard.GetComponent<Carta>();

        playerCard.Tema = cartas[num].Tema;
        playerCard.Nombre = cartas[num].Nombre;
        playerCard.Personaje = cartas[num].Personaje;
        playerCard.Pregunta = cartas[num].Pregunta;
        playerCard.Condicion = cartas[num].Condicion;
        playerCard.Probabilidad = cartas[num].Probabilidad;
        playerCard.SobrescribirSi = cartas[num].SobrescribirSi;
        playerCard.SiDinero = cartas[num].SiDinero;
        playerCard.SiGente = cartas[num].SiGente;
        playerCard.SiFlora = cartas[num].SiFlora;
        playerCard.SiFauna = cartas[num].SiFauna;
        playerCard.SiAire = cartas[num].SiAire;
        playerCard.ExtrasSi = cartas[num].ExtrasSi;
        playerCard.SobrescribeNo = cartas[num].SobrescribeNo;
        playerCard.NoDinero = cartas[num].NoDinero;
        playerCard.NoGente = cartas[num].NoGente;
        playerCard.NoFlora = cartas[num].NoFlora;
        playerCard.NoFauna = cartas[num].NoFauna;
        playerCard.NoAire = cartas[num].NoAire;
        playerCard.ExtrasNo = cartas[num].ExtrasNo;
        playerCard.TextoExplicativo = cartas[num].TextoExplicativo;

        if (playerCard.SobrescribeNo == "")
            playerCard.SobrescribeNo = "No";

        if (playerCard.SobrescribirSi == "")
            playerCard.SobrescribirSi = "Si";

        if (cartas[num].Personaje == "Faustino el agricultor")
        {
            newCard.GetComponent<Image>().sprite = sprites_[0];
        }
        else if(cartas[num].Personaje == "Toni el activista")
        {
            newCard.GetComponent<Image>().sprite = sprites_[1];
        }
        else if (cartas[num].Personaje == "Paqui Jefa de Medio Ambiente")
        {
            newCard.GetComponent<Image>().sprite = sprites_[2];
        }
        else
        {
            newCard.GetComponent<Image>().sprite = sprites_[3];
        }
    }

    public int[] GenerateFixedCardsSelection()
    {
        int[] fixedCardsSelection = new int[7];
        fixedCardsSelection[0] = SelectRandomFixedCard("Agricultura");
        fixedCardsSelection[1] = SelectRandomFixedCard("Deforestación"); ;
        fixedCardsSelection[2] = SelectRandomFixedCard("Energía eólica"); ;
        fixedCardsSelection[3] = SelectRandomFixedCard("Fabrica/Economia");
        fixedCardsSelection[4] = SelectRandomFixedCard("Ganaderia");
        fixedCardsSelection[5] = SelectRandomFixedCard("Energía Solar");
        fixedCardsSelection[6] = SelectRandomFixedCard("Prevención de incendios");

        System.Random rng = new System.Random();
        int n = fixedCardsSelection.Length;

        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            int value = fixedCardsSelection[k];
            fixedCardsSelection[k] = fixedCardsSelection[n];
            fixedCardsSelection[n] = value;
        }

        return fixedCardsSelection;
    }

    int SelectRandomFixedCard(string condicion)
    {
        List<Carta> cartasFiltradas = cartasFijas.FindAll(carta => carta.Tema == condicion);

        int indiceAleatorio = UnityEngine.Random.Range(0, cartasFiltradas.Count);
        return cartasFiltradas[indiceAleatorio].CardId;
    }

    int SelectRandomCard()
    {
        List<Carta> cartasNoFijas = cartas.FindAll(carta => carta.Condicion != "Evento fijo");

        int indiceAleatorio = -1;
        do
        {
            indiceAleatorio = UnityEngine.Random.Range(0, cartasNoFijas.Count);
        } while (!cartas[indiceAleatorio].Usada);

        return cartasNoFijas[indiceAleatorio].CardId;
    }

    void DiscardUsedCard(int id)
    {
        cartas[id].Usada = true;
    }

    //public Carta[] DevolverCartasPorId() {
    //    GenerateFixedCardsSelection[];
    //    Carta[]
    //}
}

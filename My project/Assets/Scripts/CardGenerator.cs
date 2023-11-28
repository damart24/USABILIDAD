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
    List<Carta> cartas = new List<Carta>();
    List<Carta> cartasFijas = new List<Carta>();

    private void Start()
    {
        string filePath = Path.Combine("Assets", "CSV", fileName);
        string[] lines = File.ReadAllLines(filePath);

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
                TextoExplicativo = values[20]
            };
            Debug.Log("Hola");
            cartas.Add(carta);
        }
        cartasFijas = cartas.FindAll(carta => carta.Condicion == "Evento fijo");
    }

    //Instancia la nueva carta que estará detrás del mazo
    //Le pone como primer hijo y cambia su sprite por un sprite aleatorio
    public void InstantiateCard()
    {
        GameObject newCard = Instantiate(cardPrefab_, transform, false);
        newCard.transform.SetAsFirstSibling();
        newCard.GetComponent<Image>().sprite = sprites_[UnityEngine.Random.Range(0, sprites_.Length)];
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
            indiceAleatorio = Random.Range(0, cartasNoFijas.Count);
        } while (!cartas[indiceAleatorio].Usada);

        return cartasNoFijas[indiceAleatorio].CardId;
    }

    void DiscardUsedCard(int id)
    {
        cartas[id].Usada = true;
    }
}

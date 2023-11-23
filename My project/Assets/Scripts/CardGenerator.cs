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

    //private void Start()
    //{
    //    string filePath = Path.Combine("Assets", "CSV", fileName);
    //    string[] lines = File.ReadAllLines(filePath);

    //    foreach (string line in lines)
    //    {
    //        string[] values = line.Split(',');

    //        Carta carta = new Carta
    //        {
    //            Tema = values[0],
    //            Nombre = values[1],
    //            Personaje = values[2],
    //            Pregunta = values[3],
    //            Condicion = values[4],
    //            Probabilidad = int.Parse(values[5]),
    //            SobrescribirSi = values[6],
    //            SiDinero = int.Parse(values[7]),
    //            SiGente = int.Parse(values[8]),
    //            SiFlora = int.Parse(values[9]),
    //            SiFauna = int.Parse(values[10]),
    //            SiAire = int.Parse(values[11]),
    //            ExtrasSi = values[12],
    //            SobrescribeNo = values[13],
    //            NoDinero = int.Parse(values[14]),
    //            NoGente = int.Parse(values[15]),
    //            NoFlora = int.Parse(values[16]),
    //            NoFauna = int.Parse(values[17]),
    //            NoAire = int.Parse(values[18]),
    //            ExtrasNo = values[19],
    //            TextoExplicativo = values[20]
    //        };

    //        cartas.Add(carta);
    //    }
    //}

    //Instancia la nueva carta que estará detrás del mazo
    //Le pone como primer hijo y cambia su sprite por un sprite aleatorio
    public void InstantiateCard()
    {
        GameObject newCard = Instantiate(cardPrefab_, transform, false);
        newCard.transform.SetAsFirstSibling();
        newCard.GetComponent<Image>().sprite = sprites_[Random.Range(0, 2)];
    }
}

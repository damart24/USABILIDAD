using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Carta : MonoBehaviour, System.ICloneable
{
    [HideInInspector]
    public int CardId;
    [HideInInspector]
    public bool Usada;
    [HideInInspector]
    public string Tema;
    [HideInInspector]
    public string Nombre;
    [HideInInspector]
    public string Personaje;
    [HideInInspector]
    public string Pregunta;
    [HideInInspector]
    public string Condicion;
    [HideInInspector]
    public int Probabilidad;
    [HideInInspector]
    public string SobrescribirSi;
    [HideInInspector]
    public int SiDinero;
    [HideInInspector]
    public int SiGente;
    [HideInInspector]
    public int SiFlora;
    [HideInInspector]
    public int SiFauna;
    [HideInInspector]
    public int SiAire;
    [HideInInspector]
    public string ExtrasSi;
    [HideInInspector]
    public string SobrescribeNo;
    [HideInInspector]
    public int NoDinero;
    [HideInInspector]
    public int NoGente;
    [HideInInspector]
    public int NoFlora;
    [HideInInspector]
    public int NoFauna;
    [HideInInspector]
    public int NoAire;
    [HideInInspector]
    public string ExtrasNo;
    [HideInInspector]
    public string TextoExplicativo;
    public object Clone()
    {
        return new Carta
        {
            // Copia de las propiedades simples
            CardId = this.CardId,
            Usada = this.Usada,
            Tema = this.Tema,
            Nombre = this.Nombre,
            Personaje = this.Personaje,
            Pregunta = this.Pregunta,
            Condicion = this.Condicion,
            Probabilidad = this.Probabilidad,
            SobrescribirSi = this.SobrescribirSi,
            SiDinero = this.SiDinero,
            SiGente = this.SiGente,
            SiFlora = this.SiFlora,
            SiFauna = this.SiFauna,
            SiAire = this.SiAire,
            ExtrasSi = this.ExtrasSi,
            SobrescribeNo = this.SobrescribeNo,
            NoDinero = this.NoDinero,
            NoGente = this.NoGente,
            NoFlora = this.NoFlora,
            NoFauna = this.NoFauna,
            NoAire = this.NoAire,
            ExtrasNo = this.ExtrasNo,
            TextoExplicativo = this.TextoExplicativo
        };
    }
}



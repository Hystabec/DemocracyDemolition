using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Progressbar : MonoBehaviour
{
    private Scrollbar _progressBar;
    [SerializeField]
    private TMP_Text _progressText;
    float _chargeValue;
    float _chargeSpeed;
    bool _isCharging;
    private void Charge()
    {
        _chargeValue += _chargeSpeed * Time.deltaTime;
        if (_chargeValue > 1)
        {
            _chargeValue = 1;
        }
    }
    private void Discharge()
    {
        _chargeValue -= _chargeSpeed * Time.deltaTime;
        if (_chargeValue < 0)
        {
            _chargeValue = 0;
        }
    }
    private void UpdateProgressBar()
    {
        _progressBar.size = _chargeValue;
        _progressText.text = (_chargeValue * 100).ToString("0.0") + "%";
    }
    void Update()
    {
        if(_isCharging)
        {
            Charge();
        }
        else
        {
            Discharge();
        }
        UpdateProgressBar();
    }
}
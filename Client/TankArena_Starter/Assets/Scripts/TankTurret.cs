/*
 * Copyright (c) 2017 Razeware LLC
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * Notwithstanding the foregoing, you may not use, copy, modify, merge, publish, 
 * distribute, sublicense, create a derivative work, and/or sell copies of the 
 * Software in any work that is designed, intended, or marketed for pedagogical or 
 * instructional purposes related to programming, coding, application development, 
 * or information technology.  Permission for such use, copying, modification,
 * merger, publication, distribution, sublicensing, creation of derivative works, 
 * or sale is expressly withheld.
 *    
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */

using UnityEngine;
using UnityEngine.XR;

public class TankTurret : MonoBehaviour
{

    public float turnSpeed = 180f;
    public GameObject tankTurret;
    public GameObject barrel;
    public GameObject projectile;
    private float turnInputValue;
    private bool m_TriggerClick = true;

    void Update()
    {
        
        turnInputValue = Input.GetAxis("TurretRotate");
        RotateTurret();
        //Dragon Debug        
        if (this.GetTriggerClick())
        {
            Shoot();
        }
    }

    private bool GetTriggerClick()
    {
        bool isDone;
        InputDevices.GetDeviceAtXRNode(XRNode.RightHand).TryGetFeatureValue(CommonUsages.triggerButton, out isDone);

        if (isDone)
        {
            if (m_TriggerClick)
            {
                m_TriggerClick = false;
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            m_TriggerClick = true;
            return false;
        }
    }


    void RotateTurret()
    {
        float turn = turnInputValue * turnSpeed * Time.deltaTime;
        Quaternion turnRotation = Quaternion.Euler(0f, 0f, turn);
        tankTurret.transform.rotation = (tankTurret.transform.rotation * turnRotation);
    }

    void Shoot()
    {
        Instantiate(projectile, barrel.transform.position, barrel.transform.rotation);
    }
}
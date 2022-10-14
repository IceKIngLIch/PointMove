using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;



public class movie : MonoBehaviour
{
    // Start is called before the first frame update
    private bool _isMoving = false;
    private bool _camChangePos = false;
    private Vector3 _targetPosition;
    private Vector3 _cameraTargPos;
    private float speed = 1;
    private Queue<Vector3> myQ = new Queue<Vector3>();
    public LineRenderer line = new LineRenderer();
    public Slider Speed_slider;
    private bool click;



    public void Enter()
    {
        click = true;
    }
    public void Exit()
    {
        click = false;
    }
    void Start()
    {
        click=false;
        _targetPosition = this.transform.position;
        line.positionCount--;
        line.SetPosition(0, _targetPosition);
        speed = Speed_slider.value;
    }

    // Update is called once per frame
    void Update()
    {

        //изменение скорости слайдером
        speed = Speed_slider.value;


        if (Input.GetMouseButtonDown(0) && click == false)// mouse up изза чтобы не было кофликтов со скролбаром
        {
            Targetposition();
        }
            if (_isMoving)// двигаем объект
                Move();
            else
            {
                if (myQ.Count != 0)//если движение закончилось но очередь точек движения не закончилась перезапускаем функцию движения к точке из очереди

                {
                    _targetPosition = myQ.Dequeue();
                    _isMoving = true;
                }
            }
            if (_camChangePos)//двигаем камеру
            {
                CamMove();
            }

        }
    
    
    public void DrawPath( Vector3 P2)
    {
        
        line.positionCount++;
        line.SetPosition(line.positionCount-1,P2);  
       
    }
    private void Targetposition()
    {
        Vector3 movePoint= Camera.main.ScreenToWorldPoint(Input.mousePosition);
        movePoint.z= transform.position.z;




        if (_isMoving == true)//если обект в движение добавляем точку в очередь
        {
            DrawPath(movePoint);
            myQ.Enqueue(movePoint);            
        }
        else
        {
            DrawPath( movePoint);
            _targetPosition = movePoint;
            _isMoving = true;
        }   
    }
    private void CamMove()
    {
        if(Camera.main.transform.position== _cameraTargPos)
            _camChangePos = false;
        Camera.main.transform.position = Vector3.MoveTowards(Camera.main.transform.position, _cameraTargPos, speed * Time.deltaTime*2);//двигаем камеру в заданную точку

    }
    private void RemoveLine()
    {
        List<Vector3> point = new List<Vector3>();
        for (int i = 0; i < line.positionCount; i++)
        { point.Add(line.GetPosition(i)); };
        for (int i = 0; i < point.Count - 1; i++)
            line.SetPosition(i, point[i + 1]);
        line.positionCount--;
    }
    private void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position,_targetPosition,speed*Time.deltaTime);//двигаем обект в заданную точку
        if (transform.position == _targetPosition)//попадает в точку назначения и должен начать двигать камеру
        {
            RemoveLine();
                      
            _isMoving = false;
            _cameraTargPos = new Vector3(transform.position.x,transform.position.y,-100);
            _camChangePos = true;
            
        }
    }

   
}

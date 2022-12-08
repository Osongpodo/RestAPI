RestAPI
===
> **Unity 개발 시 Rest API 호출하는 방법**   
[EDT Rest API Test Server](http://106.247.250.251:31866/)

![Untitled](https://user-images.githubusercontent.com/73912947/201695219-d56667e2-0a6e-4d57-b480-5d74dca708d0.png)
## 🔎 RestAPI?
REpresentational State  Transfer 의 약자로, Http 프로토콜로 데이터를 전달하고 웹 서비스를 만들기 위한 아키텍처 패턴이다.

## 💡 RestAPI 특징
- **Uniform Interface**
: Uniform Interface는 URI로 지정한 리소스에 대한 조작을 통일되고 한정적인 인터페이스로 수행하는 아키텍처 스타일
- **Stateless**
: REST는 무상태성 성격을 가진다. 다시 말해 작업을 위한 상태정보를 따로 저장하고 관리하지 않는다. 세션 정보나 쿠키정보를 별도로 저장하고 관리하지 않기 때문에 API 서버는 들어오는 요청만을 단순히 처리하면 된다. 때문에 서비스의 자유도가 높아지고 서버에서 불필요한 정보를 관리하지 않음으로써 구현이 단순해진다.
- **Cacheable**
: REST의 가장 큰 특징 중 하나는 HTTP라는 기존 웹표준을 그대로 사용하기 때문에, 웹에서 사용하는 기존 인프라를 그대로 활용이 가능하다. 따라서 HTTP가 가진 캐싱 기능이 적용 가능하다. HTTP 프로토콜 표준에서 사용하는 Last-Modified태그나 E-Tag를 이용하면 캐싱 구현이 가능하다.
- **Self-descriptiveness**
: REST API 메시지만 보고도 이를 쉽게 이해 할 수 있는 자체 표현 구조로 되어 있다.
- **Client-Server**
: REST 서버는 API 제공, 클라이언트는 사용자 인증이나 컨텍스트(세션, 로그인 정보)등을 직접 관리하는 구조로 각각의 역할이 확실히 구분되기 때문에 클라이언트와 서버에서 개발해야 할 내용이 명확해지고 서로간 의존성이 줄어들게 된다.
- **Layered System**
: REST 서버는 다중 계층으로 구성될 수 있으며 보안, 로드 밸런싱, 암호화 계층을 추가해 구조상의 유연성을 둘 수 있고 PROXY, 게이트웨이 같은 네트워크 기반의 중간매체를 사용할 수 있게 한다.

## 💻 HTTP 통신
- **코루틴을 사용**
: 웹서버로부터 정보를 주고받는데 소요되는 시간이 필요하기 때문에 코루틴을 대체적으로 많이 쓴다.
- **using문을 사용**
: 웹서버를 통해서 다양한 리소스를 사용하게 될 텐데, 리소스를 사용하게 되면 적절한 시기에 Dispose 처리를 해줘야 자원관리가 제대로 이뤄지는데 매번 수동으로 dispose를 할 순 없으니 using문을 작성하고 내부에서 리소스를 사용하게 되면 자동으로 dispose 해주기 때문에 자원관리에 용이해진다.

|방식|특징|
|:---:|:---:|
|GET|요청 데이터의 정보를 주소에 담아서 전송하는 방식|
|POST|요청 데이터의 정보를 HTTP헤더를 통해 전달하는 방식|

📌 POST 방식이 GET 방식보다 조금 더 보안성이 있고 많은 데이터를 전송할 수 있지만 다소 느리다.

## 🔐 UnityWebRequest 방식을 활용한 HTTP 통신
> using UnityEngine.Networking 선언 필수 !

``` c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkTest : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(UnityWebRequestGETTest());
    }

    IEnumerator UnityWebRequestGETTest()
    {
        string url = "http://106.247.250.251:31866/read_ints";

        UnityWebRequest www = UnityWebRequest.Get(url);

        yield return www.SendWebRequest();

        if(www.error == null)
        {
            Debug.Log(www.downloadHandler.text);
        }
        else
        {
            Debug.Log("error");
        }
    }
}
```

GET 방식을 이용하여 해당 서버의 Int_Table을 출력하였다.   
## 📋 Json Parsing
> Callback 함수를 이용하여 두 스크립트 간 코드를 작성하였고 (새롭게 알게 된 사실!ㅎㅎ)   
> Server의 Json 문법으로 적힌 string을 객체로 바꿔 읽어온다.   
> Value vv = JsonUtility.FromJson<Value>(str);   
📌 **Json 직렬화는 구조화된 Json 개념을 사용한다. 즉 Json 데이터에 저장하려는 변수를 설명하는 클래스 또는 구조를 만들어야한다.**
```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class JsonTest : MonoBehaviour
{
    public delegate void callback(string str);

    public void LoadData(callback func)
    {
        StartCoroutine(UnityWebRequestGETTest(func));
    }

    IEnumerator UnityWebRequestGETTest(callback func)
    {
        string url = "http://106.247.250.251:31866/read_ints";

        UnityWebRequest www = UnityWebRequest.Get(url);

        yield return www.SendWebRequest();

        if (www.error == null)
        {
            func(www.downloadHandler.text);
        }
        else
        {
            Debug.Log("error");
        }
    }
}
```
```c#
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCode : MonoBehaviour
{
    public JsonTest test;

    void Start()
    {
        test.LoadData(printLog);

    }

    public void printLog(string str)
    {
        Value vv = JsonUtility.FromJson<Value>(str);

        foreach(Data data in vv.int_table)
        {
            data.printData();
            Debug.Log("====================================");
        }
    }
}

// 
[Serializable]
public class Data
{
    public int id;
    public string logging_time;
    public int int_value;

    public void printData()
    {
        Debug.Log("id : " + id);
        Debug.Log("logging_time : " + logging_time);
        Debug.Log("int_value : " + int_value);
    }
}

[Serializable]
public class Value
{
    public Data[] int_table;
}

```
## 🍇 결과
> Canvas UI도 제작하고, Network도 연결하고, 데이터도 받아왔다!
> 이제 Index에 데이터가 이쁘게 나오도록 한다!   

![image](https://user-images.githubusercontent.com/73912947/206358662-7b531317-3785-410d-9360-4a7cbfc15865.png)

## 📊 Making Graph
> Table에 맞는 Data의 값에 맞게  Graph를 그리도록 한다. (우선은 id까지 출력! value에 맞게 그래프 제작하기)   
    
![image](https://user-images.githubusercontent.com/73912947/206384796-62ba1e92-749d-43df-8c62-261caa9ade51.png)


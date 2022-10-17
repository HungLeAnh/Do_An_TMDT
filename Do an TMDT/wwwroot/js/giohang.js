function dolen(x)
       {
        let y=Number(document.getElementById("trangyeuthich").value);
        if(x.style.color=="red")
        {
            x.style.color="black";
        
        y-=1;
        document.getElementById("trangyeuthich").value=y;
        }
        else{
            x.style.color="red";
        
        y+=1;
        document.getElementById("trangyeuthich").value=y; 
        }
       }
const btn=document.querySelectorAll("button")
//console.log(btn);
btn.forEach(function(button,index){
   // console.log(button,index)
   button.addEventListener("click",function(event){
    {
         var btnitem = event.target
        var product=btnitem.parentElement
        var layer=product.parentElement
        var item=layer.parentElement
        var itemimg=item.querySelector("img").src
        var itemten =item.querySelector("a").innerText
        var itemgia =item.querySelector("div.item_gia")
        var gia=itemgia.querySelector("a").innerText
        var Sanpham=new Object();
        Sanpham.Hinhanh=itemimg;
        Sanpham.Ten=itemten;
        Sanpham.Gia=gia;
        var danhsachsanpham=new Array();


        danhsachsanpham.push(Sanpham)
        console.log(danhsachsanpham)
        danhsachsanpham.toJson=function()
        {
            var json=stringfy(this)
            return json;
        }
        localStorage.setItem('danhsachsanpham',danhsachsanpham)

        Themgio(Sanpham)
    }
   })
})
function Themgio(Sanpham)
{
   
    var addtr=document.createElement("tr")  
    var trnoidung='<tr><td><img src="'+Sanpham.Hinhanh+'"  alt=""></td><td>'+Sanpham.Ten+'</td><td><p><span>'+Sanpham.Gia+'</span><sup>đ</sup></p></td><td><input type="number" value="1" min="0"></td><td>xóa</td></tr>'
    addtr.innerHTML=trnoidung
    var taotr=document.querySelector("tbody")
    taotr.append(addtr)

}
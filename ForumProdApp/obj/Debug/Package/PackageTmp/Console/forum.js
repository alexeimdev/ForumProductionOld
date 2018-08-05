var tableValues;
var done_tableValues;

$(document).on("pageshow", "#page1", function () { 
     var geturl;
    $('#popupBasic').popup("open");

    if(!window.navigator.onLine){
            alert ("אין חיבור לאינטרנט - לא תוכל לקבל נתונים");
            $('#popupBasic').popup("close");
    }
    else
    {
       
    
     $.ajax({
            type: "POST",
            url: "forumProd.asmx/getName",
            data: {},
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                
                if(JSON.parse(msg.d) == "notAllowed") {
               $('#popupBasic').popup("close");
                    alert('אין לך הרשאות  - אנא פנה לצוות כלים אוטומטים');
                     
                     $.mobile.changePage("#notAllowed", {
                        transition: "none"});
                }
                else if(JSON.parse(msg.d) == "ErrorFromServer") {
                    alert('כשלון בפניה באימות נתונים - נא בדוק התחברות');
                }
                else{
                   $('#page1_id').text(JSON.parse(msg.d));
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert(' getName - כשלון בפניה לשרת - אנא פנה לצוות כלים אוטומטים');
            }

        });

                    $.ajax({
                        type: "POST",
                        url: "forumProd.asmx/GetForumForMe",
                        data: "{}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (msg) {
                            // Replace the div's content with the page method's return.
                            var lstTweets = $('#lstTweets');
                            var tableValues = $.parseJSON(msg.d);
                            localStorage.tableValues = msg.d;
                            
                            lstTweets.children().remove('li');

                            localStorage.amntofreqstodo = tableValues.length;
                            $('#howmanyreqsTodo_1').text(tableValues.length);
                            $('#numberofRequests_lbl').text(tableValues.length);
                            

                            for (var i = 0; i < tableValues.length; i++) {
                             /*   if (!Isthere('#' + tableValues[i].BG_BUG_ID) || sessionStorage.resetDone == "1") {
                                    if(sessionStorage.resetDone == "1") {
                                        sessionStorage.resetDone = "2"; 
                                    }*/
                                    lstTweets.append($('<li id=' + tableValues[i].BG_BUG_ID + '><a href="#forumRequest_def" id=' + tableValues[i].BG_BUG_ID + '_lnk data-role="button" data-icon="arrow-r" data-transition="none"  data-iconpos="right" >' + tableValues[i].BG_BUG_ID + '</a></li>'));
                              /*  }*/
               
                            }

                            if (tableValues.length == 0) {
                                lstTweets.children().remove('li');
                            }

                           /* if (sessionStorage.RequestDone != "") {
                                  $('#' + sessionStorage.RequestDone).remove();
                            }*/


                            lstTweets.listview('refresh');

                            sessionStorage.RequestDone = "";
                           

                            $('#lstTweets').on('click', 'li', function () {
                                sessionStorage.bugIDClicked = $(this).attr('id');
                                sessionStorage.done = "todolist";
                            });

                        },
                        error: function (xhr, ajaxOptions, thrownError) {
                          
                        }

                    });

                     $.ajax({
                        type: "POST",
                        url: "forumProd.asmx/GetDoneForumForMe",
                        data: "{}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (msg) {
                            // Replace the div's content with the page method's return.
                            var doneReqs = $('#requestsDone_list');
                            var DonetableValues = $.parseJSON(msg.d);
                            localStorage.DonetableValues = msg.d;
                            $('#numberofRequestsdone_lbl').text(DonetableValues.length);
                             doneReqs.children().remove('li');

                            /*if (DonetableValues.length == 0) {
                                doneReqs.children().remove('li');
                            }
                            else if(sessionStorage.resetDone == "2") {
                                doneReqs.empty();
                                sessionStorage.resetDone = "0"
                                doneReqs.listview('refresh');
                            }*/
                            
                            for (var i = 0; i < DonetableValues.length; i++) {
                               // if (!Isthere('#' + DonetableValues[i].BG_BUG_ID) ) {
                                    doneReqs.append($('<li id=' + DonetableValues[i].BG_BUG_ID + '><a href="#forumRequest_def" id=' + DonetableValues[i].BG_BUG_ID + '_lnk data-role="button" data-icon="check" data-transition="none"  data-iconpos="right" >' + DonetableValues[i].BG_BUG_ID + '</a></li>'));
                                //}
                            }

                           

                            doneReqs.listview('refresh');

                             $('#popupBasic').popup("close");
                            

                            $('#requestsDone_list').on('click', 'li', function () {
                                sessionStorage.bugIDClicked = $(this).attr('id');
                                sessionStorage.done = "donelist";
                            });

                           $('#popupBasic').popup("close");
                        },
                        error: function (xhr, ajaxOptions, thrownError) {
                              $('#popupBasic').popup("close");
                              alert('כשלון בפניה לשרת - אנא פנה לצוות כלים אוטומטים - GetDoneForumForMe');
                        }

                    });

                    
}
    $('#popupBasic').popup("close");
});


/////////////////////////////////////////////////////////////////////////////////////////////////////////////////


function tableValuesCreate(tableValues,idRequChange) {  

    for (var i = 0; i < tableValues.length; i++) {
        if (tableValues[i].BG_BUG_ID == idRequChange) {
            //תיאור
            $('#BG_PROJECT').text(tableValues[i].BG_PROJECT);
            $('#BG_SUMMARY').text(tableValues[i].BG_SUMMARY);
            $('#BG_RESPONSIBLE').text(tableValues[i].BG_RESPONSIBLE);
            $('#BG_DETECTED_BY').text(tableValues[i].BG_DETECTED_BY);
            $('#BG_USER_01').text(tableValues[i].BG_USER_01); //סוג בקשה
            
            // תיאור //
            if(tableValues[i].BG_DESCRIPTION == null) {
                    $('#BG_DESCRIPTION').text("שדה תיאור ללא תוכן ");
            }else {
                    $('#BG_DESCRIPTION').text(strip(tableValues[i].BG_DESCRIPTION)); //תיאור
            }

            // אישורים //
            var tbd = "טרם עודכן גורם מאשר";
          
            //אישור QA
            if (tableValues[i].BG_USER_23 == "כן") {
                $("#BG_USER_23_lbl").text('אישור ' + tableValues[i].BG_USER_02 + ' :QA');
                $("#BG_USER_23").attr("src","img/check.png");
            }
            else if (tableValues[i].BG_USER_23 == "לא") {
                $("#BG_USER_23_lbl").text('אישור ' + tableValues[i].BG_USER_02 + ' :QA');
                $("#BG_USER_23").attr("src","img/nope.png");
            }
            else if (tableValues[i].BG_USER_23 == "טנטטיבי") {
                $("#BG_USER_23_lbl").text('אישור ' + tableValues[i].BG_USER_02 + ' :QA');
                $("#BG_USER_23").attr("src","img/tentative.png");
            }
            else if (tableValues[i].BG_USER_23 == "נבדק על ידי הפיתוח") {
                $("#BG_USER_23_lbl").text('אישור ' + tableValues[i].BG_USER_02 + ' :QA');
                $("#BG_USER_23").attr("src","img/bullet.png");

            }
            else
            {
                if(tableValues[i].BG_USER_02 == null) {
                        $("#BG_USER_23_lbl").text('אישור QA: ' + tbd);
                        $("#BG_USER_23").attr("src","img/dont-know-icon.png");
                }
                else
                {
                    $("#BG_USER_23_lbl").text('אישור ' + tableValues[i].BG_USER_02 + ' :QA');
                     $("#BG_USER_23").attr("src","img/dont-know-icon.png");
                }

            }

            // אישור ראש צוות
            if (tableValues[i].BG_USER_43 == "כן") {
                $("#BG_USER_43_lbl").text('אישור ראש צוות: ' + tableValues[i].BG_USER_42);
                $("#BG_USER_43").attr("src","img/check.png");
            }
            else if (tableValues[i].BG_USER_43 == "לא") {
                $("#BG_USER_43_lbl").text('אישור ראש צוות: ' + tableValues[i].BG_USER_42);
                $("#BG_USER_43").attr("src","img/nope.png");
            }
            else
            {
                if(tableValues[i].BG_USER_42 == null) {
                    $("#BG_USER_43_lbl").text('אישור ראש צוות:' + tbd);
                    $("#BG_USER_43").attr("src","img/dont-know-icon.png");
                }
                else
                {
                    $("#BG_USER_43_lbl").text('אישור ראש צוות: ' + tableValues[i].BG_USER_42);
                    $("#BG_USER_43").attr("src","img/dont-know-icon.png");
                }
                
            }

            // אישור DBA
            if (tableValues[i].BG_USER_24 == "כן") {
                $("#BG_USER_24_lbl").text('אישור ' + tableValues[i].BG_USER_03 + ' :DBA');
                $("#BG_USER_24").attr("src","img/check.png");
            }
            else if (tableValues[i].BG_USER_24 == "לא") {
                $("#BG_USER_24_lbl").text('אישור ' + tableValues[i].BG_USER_03 + ' :DBA');
                $("#BG_USER_24").attr("src","img/nope.png");  
            }
            else
            {   
                $("#BG_USER_24").attr("src","img/dont-know-icon.png");
                if(tableValues[i].BG_USER_03 == null) {
                    $("#BG_USER_24_lbl").text('אישור DBA: ' + tbd);   
                }
                else
                {
                    $("#BG_USER_24_lbl").text('אישור ' + tableValues[i].BG_USER_03 + ' :DBA');
                }
            }

            ////מנהל תחום///
            if (tableValues[i].BG_USER_21 == "כן") {
                $("#BG_USER_21_lbl").text('אישור מנהל תחום: ' + tableValues[i].BG_USER_09);
                $("#BG_USER_21").attr("src","img/check.png");
            }
            else if (tableValues[i].BG_USER_21 == "לא") {
                $("#BG_USER_21_lbl").text('אישור מנהל תחום: ' + tableValues[i].BG_USER_09);
                $("#BG_USER_21").attr("src","img/nope.png");
            }
            else
            {
                $("#BG_USER_21").attr("src","img/dont-know-icon.png");
                if(tableValues[i].BG_USER_09 == null) {
                    $("#BG_USER_21_lbl").text('אישור מנהל תחום: ' + tbd);
                }
                else
                {
                    $("#BG_USER_21_lbl").text('אישור מנהל תחום: ' + tableValues[i].BG_USER_09);
                }   
            }

            //אבטחת מידע
            if (tableValues[i].BG_USER_26 == "כן") {
                 $("#BG_USER_26_lbl").text('אישור אבטחת מידע: ' + tableValues[i].BG_USER_10);
                $("#BG_USER_26").attr("src","img/check.png");
            }
            else if (tableValues[i].BG_USER_26 == "לא") {
                $("#BG_USER_26_lbl").text('אישור אבטחת מידע: ' + tableValues[i].BG_USER_10);
                $("#BG_USER_26").attr("src","img/nope.png");
            }
            else
            {
              $("#BG_USER_26").attr("src","img/dont-know-icon.png");
                if(tableValues[i].BG_USER_10 == null) {
                    $("#BG_USER_26_lbl").text('אישור אבטחת מידע: ' + tbd);
                }
                else
                {
                    $("#BG_USER_26_lbl").text('אישור אבטחת מידע: ' + tableValues[i].BG_USER_10);
                }
            }


            //אישור תפעול

            if (tableValues[i].BG_USER_27 == "כן") {
                $("#BG_USER_27_lbl").text('אישור תפעול: ' + tableValues[i].BG_USER_11);
                  $("#BG_USER_27").attr("src","img/check.png");
            }
            else if (tableValues[i].BG_USER_27 == "לא") {
                $("#BG_USER_27_lbl").text('אישור תפעול: ' + tableValues[i].BG_USER_11);
                  $("#BG_USER_27").attr("src","img/nope.png");
            }
            else
            {
               $("#BG_USER_27").attr("src","img/dont-know-icon.png");
                if(tableValues[i].BG_USER_11 == null) {
                    $("#BG_USER_27_lbl").text('אישור תפעול: ' + tbd);
                }
                else
                {
                    $("#BG_USER_27_lbl").text('אישור תפעול: ' + tableValues[i].BG_USER_11);
                }
            }

            // TUX
            if (tableValues[i].BG_USER_75 == "כן") {
                $("#BG_USER_75_lbl").text('אישור ' + tableValues[i].BG_USER_76 + ' :TUX');
                 $("#BG_USER_75").attr("src","img/check.png");
            }
            if (tableValues[i].BG_USER_75 == "לא") {
                $("#BG_USER_75_lbl").text('אישור ' + tableValues[i].BG_USER_76 + ' :TUX');
                 $("#BG_USER_75").attr("src","img/nope.png");
            }
            else
            {
                $("#BG_USER_75").attr("src","img/dont-know-icon.png");
                if(tableValues[i].BG_USER_76 == null) {
                    $("#BG_USER_75_lbl").text('אישור TUX: ' + tbd);
                }
                else
                {
                    $("#BG_USER_75_lbl").text('אישור ' + tableValues[i].BG_USER_76 + ' :TUX');
                }
            }
    

            // מנהל מחלקה
             if (tableValues[i].BG_USER_22 == "כן") {
                $("#BG_USER_22_lbl").text('אישור מנהל מחלקה: ' + tableValues[i].BG_USER_04);
                 $("#BG_USER_22").attr("src","img/check.png");
            }
            else if (tableValues[i].BG_USER_22 == "לא") {
                $("#BG_USER_22_lbl").text('אישור מנהל מחלקה: ' + tableValues[i].BG_USER_04);
                 $("#BG_USER_22").attr("src","img/nope.png");
            }
            else
            {
                $("#BG_USER_22").attr("src","img/dont-know-icon.png");
                if(tableValues[i].BG_USER_04 == null) {
                    $("#BG_USER_22_lbl").text('אישור מנהל מחלקה: ' + tbd);
                }
                else
                {
                    $("#BG_USER_22_lbl").text('אישור מנהל מחלקה: ' + tableValues[i].BG_USER_04);
                }
            }
            
          
        
            //תאריכים
            var dateinForum = tableValues[i].BG_DETECTION_DATE.toString().replace("T00:00:00", "").replace("00:00:00","");
            datinForumarr =  dateinForum.split('-');
            tmpint = parseInt(datinForumarr[2].toString());

            if(tmpint >= 50)
            {
                $('#BG_DETECTION_DATE').text(datinForumarr[0] + "/" +  datinForumarr[1] + "/" + datinForumarr[2]);
            }
            else
            {
                $('#BG_DETECTION_DATE').text(datinForumarr[2] + "/" +  datinForumarr[1] + "/" + datinForumarr[0]);
            }

            
           
           
            
            

           if (tableValues[i].BG_USER_74 == null) {
                $('#BG_USER_74').text('TBD');
            }
            else {
             
                   dateArr = tableValues[i].BG_USER_74.split('-');
                   tmpint = parseInt(dateArr[2].toString());

                    if(tmpint >= 50)
                    {
                              $('#BG_USER_74').text(dateArr[0] + "/" +  dateArr[1] + "/" + dateArr[2]);
                    }
                    else
                    {
                              $('#BG_USER_74').text(dateArr[2] + "/" +  dateArr[1] + "/" + dateArr[0]);
                    }
                  
           }

            break;
        }

    }
}

//////////////////////////////-- forumRequest_def --////////////////////////////////////////////////////////
$(document).on("pageshow", "#forumRequest_def", function () { 
//$('#forumRequest_def').on('pageshow', function () {
     $('#loadingDiv').show()
     $("#refreshDoneDiv").hide();
     
    var idRequChange = sessionStorage.bugIDClicked;
    if(idRequChange.indexOf("Done") > 0) {
        $('#back_Done_Div').show();
        idRequChange = idRequChange.replace("_Done","");   
         $('#reset_desc_div').hide();
         $('#refreshandBack_div').hide();
         $('#buttons_div').hide();
          
         var tableValues;
         $('#requestID').text('מספר בקשה : ' + idRequChange);
         tableValues = $.parseJSON(localStorage.tableValues);
    }
    else {
    
            if(sessionStorage.bugIDClicked == "") {
          
                $.mobile.changePage( "#page1", { transition: "none", changeHash: true });
                sessionStorage.bugIDClicked = "";
            }
            else {
                    $('#back_Done_Div').hide();
                    var tableValues;
                    $('#requestID').text('מספר בקשה : ' + idRequChange);

                    if(sessionStorage.done == "donelist") {
                       tableValues = $.parseJSON(localStorage.DonetableValues);
                       $('#notApproved_btn').attr("disabled","disabled");
                       $('#Approved_btn').attr("disabled","disabled");
      
                      $('#buttons_div').hide();
                       $('#refreshandBack_div').show();
                     $('#reset_desc_div').show();
                     }else {
					   tableValues = $.parseJSON(localStorage.tableValues);
					   $('#notApproved_btn').removeAttr("disabled");
					   $('#Approved_btn').removeAttr("disabled");
		  
					   $('#buttons_div').show();
					   $('#refreshandBack_div').show();
					   $('#reset_desc_div').hide();
   
					 }
            }
     }

    tableValuesCreate(tableValues,idRequChange);
    $('#loadingDiv').hide();
 
    
     $.ajax({
            type: "POST",
            url: "forumProd.asmx/getRole",
            data: "{requestID:'" + sessionStorage.bugIDClicked + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                
                if(JSON.parse(msg.d) == "notAllowed") {
               $('#popupBasic').popup("close");
                    alert('אין לך הרשאות  - אנא פנה לצוות כלים אוטומטים');
                     
                     $.mobile.changePage("#notAllowed", {
                        transition: "none"});
                }
                else if(JSON.parse(msg.d) == "ErrorFromServer") {
                    alert('כשלון בפניה באימות נתונים - נא בדוק התחברות');
                }
                else {
                
                   var role = JSON.parse(msg.d)
                   var roles = role.split(':');
                   
                   if (roles[0] == "tlQA" ) {
                        $('#tentative_div').show();
                   }
                   else {
                         $('#tentative_div').hide();
                   }

                   sessionStorage.NumberofRoles = roles[1].toString();
                   
                           
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert('getRole - כשלון בפניה לשרת - אנא פנה לצוות כלים אוטומטים');
            }

        });



    $('#cancel_goBack_lnk').unbind('click').click(function () {
        
        $.mobile.changePage( "#page1", { transition: "slideup", changeHash: true });
        sessionStorage.bugIDClicked = "";
        
    });

    
    $('#back_btn_approved').unbind('click').click(function () {
        
        $.mobile.changePage( "#approvedForAll", { transition: "slideup", changeHash: true });
        sessionStorage.bugIDClicked = "";
        
    });

    $('#notApproved_btn').unbind('click').click(function () {
        var req = sessionStorage.bugIDClicked;
        var res = "לא";

        if(parseInt(sessionStorage.NumberofRoles) > 1) {
            var currsrc = $("#BG_USER_23").attr('src');
            if( currsrc == 'img/bullet.png' || currsrc == 'img/tentative.png' )
            {
                sessionStorage.currtent = currsrc;
                sessionStorage.btnClicked = 'לא'
                $('#tentDiv').popup("open");
            }
        }
        else
        {
        
            $('#loadingDiv').show()
       
            $.ajax({
                type: "POST",
                url: "forumProd.asmx/ApproveRequest",
                data: "{requestID:'" + req + "',statuschange: '" + res + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
               success: function (msg) {
                    if (msg.d == "\"Fail\"") {
                        alert('טיפול בבקשה לא הצליח - נא לפנות לצוות כלים אוטומטים');
                    }
                    if (msg.d == "\"Success\"" || msg.d == "\"Warning\"") {
                        alert('בקשת פורום ' + sessionStorage.bugIDClicked + ' נדחתה');
                        sessionStorage.RequestDone = sessionStorage.bugIDClicked;
                        sessionStorage.bugIDClicked = "";
                        $.mobile.changePage("#page1", {
                            transition: "slideup",
                        });
                    }
                   $('#loadingDiv').popup("close");
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    $('#loadingDiv').popup("close");
                    alert('כשלון בפניה לשרת - אנא פנה לצוות כלים אוטומטים');
                }

            });
        }
      
    });

    function refreshPage(page){
    // Page refresh
             page.trigger('pagecreate');
             page.listview('refresh');
    }


    $('#canceltheTent_btn').unbind('click').click(function () {
            var req = sessionStorage.bugIDClicked;
            var res = sessionStorage.btnClicked;
            var shunta = "";
            if (res == 'כן') {
                shunta = "אושרה";
            }
            else {
               shunta = "נדחה";
            }

           $('#tentDiv').popup('close');
           $('#loadingDiv').popup("open");
                $.ajax({
                    type: "POST",
                    url: "forumProd.asmx/TentException",
                    data: "{requestID:'" + req + "',statuschange: '" + res + "',currTentStatus: '" +  sessionStorage.currtent + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        if (msg.d == "\"Fail\"") {
                            alert('טיפול בבקשה לא הצליח - נא לפנות לצוות כלים אוטומטים');
                        }
                        if (msg.d == "\"Success\"" || msg.d == "\"Warning\"") {
                            alert('בקשת פורום ' + sessionStorage.bugIDClicked + ' ' + shunta);
                            sessionStorage.RequestDone = sessionStorage.bugIDClicked;
                            sessionStorage.bugIDClicked = "";
                            sessionStorage.btnClicked = "";
                            $.mobile.changePage("#page1", {
                                transition: "slideup",
                            });
                        }
                       $('#loadingDiv').popup("close");
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        $('#loadingDiv').popup("close");
                        alert('כשלון בפניה לשרת - אנא פנה לצוות כלים אוטומטים');
                    }

                });
    });

    $('#runover_btn').unbind('click').click(function () {
          var req = sessionStorage.bugIDClicked;
            var res = sessionStorage.btnClicked;
            var shunta = "";
            if (res == 'כן') {
                shunta = "אושרה";
            }
            else {
               shunta = "נדחה";
            }
         $('#tentDiv').popup('close');
         $('#loadingDiv').popup("open");
                $.ajax({
                    type: "POST",
                    url: "forumProd.asmx/ApproveRequest",
                    data: "{requestID:'" + req + "',statuschange: '" + res + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        if (msg.d == "\"Fail\"") {
                            alert('טיפול בבקשה לא הצליח - נא לפנות לצוות כלים אוטומטים');
                        }
                        if (msg.d == "\"Success\"" || msg.d == "\"Warning\"") {
                            $('#tentDiv').popup("close");
                            alert('בקשת פורום ' + sessionStorage.bugIDClicked + ' ' + shunta);
                            sessionStorage.RequestDone = sessionStorage.bugIDClicked;
                            sessionStorage.bugIDClicked = "";
                            sessionStorage.btnClicked = "";

                            $.mobile.changePage("#page1", {
                                transition: "slideup",
                            });
                        }
                       $('#loadingDiv').popup("close");
                      
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        $('#loadingDiv').popup("close");
                        alert('כשלון בפניה לשרת - אנא פנה לצוות כלים אוטומטים');
                    }

                });
    });
    


    $('#Approved_btn').unbind('click').click(function () {
        var req = sessionStorage.bugIDClicked;
        var res = "כן";
        //        $("#BG_USER_23").attr("src","img/tentative.png");
        if(parseInt(sessionStorage.NumberofRoles) > 1) {
            var currsrc = $("#BG_USER_23").attr('src');
            if( currsrc == 'img/bullet.png' || currsrc == 'img/tentative.png' )
            {
                sessionStorage.currtent = currsrc;
                sessionStorage.btnClicked = 'כן'
                $('#tentDiv').popup("open");
            }
        }
        else
        {
                $('#loadingDiv').popup("open");
                $.ajax({
                    type: "POST",
                    url: "forumProd.asmx/ApproveRequest",
                    data: "{requestID:'" + req + "',statuschange: '" + res + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        if (msg.d == "\"Fail\"") {
                            alert('טיפול בבקשה לא הצליח - נא לפנות לצוות כלים אוטומטים');
                        }
                        if (msg.d == "\"Success\"" || msg.d == "\"Warning\"") {
                            alert('בקשת פורום ' + sessionStorage.bugIDClicked + ' אושרה');
                            sessionStorage.RequestDone = sessionStorage.bugIDClicked;
                            sessionStorage.bugIDClicked = "";
                            $.mobile.changePage("#page1", {
                                transition: "slideup",
                            });
                        }
                       $('#loadingDiv').popup("close");
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        $('#loadingDiv').popup("close");
                        alert('כשלון בפניה לשרת - אנא פנה לצוות כלים אוטומטים');
                    }

                });
         }
    });


    $('#tentative_btn').unbind('click').click(function () {
        var req = sessionStorage.bugIDClicked;
        var res = "טנטטיבי";
        $('#loadingDiv').popup("open");
        $.ajax({
            type: "POST",
            url: "forumProd.asmx/ApproveRequest",
            data: "{requestID:'" + req + "',statuschange: '" + res + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                if (msg.d == "\"Fail\"") {
                    alert('טיפול בבקשה לא הצליח - נא לפנות לצוות כלים אוטומטים');
                }
                if (msg.d == "\"Success\"" || msg.d == "\"Warning\"") {
                    alert('בקשת פורום ' + sessionStorage.bugIDClicked + ' אושרה טנטטיבי');
                    sessionStorage.RequestDone = sessionStorage.bugIDClicked;
                    sessionStorage.bugIDClicked = "";
                    $.mobile.changePage("#page1", {
                        transition: "slideup",
                    });
                }
               $('#loadingDiv').popup("close");
            },
            error: function (xhr, ajaxOptions, thrownError) {
                $('#loadingDiv').popup("close");
                alert('כשלון בפניה לשרת - אנא פנה לצוות כלים אוטומטים');
            }

        });
    });

     $('#dev_checked_btn').unbind('click').click(function () {
        var req = sessionStorage.bugIDClicked;
        var res = "נבדק על ידי הפיתוח";
        $('#loadingDiv').popup("open");
        $.ajax({
            type: "POST",
            url: "forumProd.asmx/ApproveRequest",
            data: "{requestID:'" + req + "',statuschange: '" + res + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                if (msg.d == "\"Fail\"") {
                    alert('טיפול בבקשה לא הצליח - נא לפנות לצוות כלים אוטומטים');
                }
                if (msg.d == "\"Success\"" || msg.d == "\"Warning\"") {
                    alert('בקשת פורום ' + sessionStorage.bugIDClicked + ' אושרה טנטטיבי');
                    sessionStorage.RequestDone = sessionStorage.bugIDClicked;
                    sessionStorage.bugIDClicked = "";
                    $.mobile.changePage("#page1", {
                        transition: "slideup",
                    });
                }
               $('#loadingDiv').popup("close");
            },
            error: function (xhr, ajaxOptions, thrownError) {
                $('#loadingDiv').popup("close");
                alert('כשלון בפניה לשרת - אנא פנה לצוות כלים אוטומטים');
            }

        });
    });

     //$('#refresh_data_btn').on("click",(function () {
     $('#refresh_data_btn').unbind('click').click(function () {
        var req = sessionStorage.bugIDClicked;
        
        $('#loadingDiv').popup("open");

        $.ajax({
            type: "POST",
            cache: false,
            url: "forumProd.asmx/RefreshData",
            data: "{requestID:'" + req + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
              
              var tableValues = $.parseJSON(msg.d);
              localStorage.tableValues = msg.d;
              $('input:checkbox').removeAttr('checked');
              $('input:checkbox').checkboxradio("refresh");
                
              tableValuesCreate(tableValues,req);
                
             
              $('#loadingDiv').popup('close');     
              $('#therequest_div').trigger('create');
              $('#forumRequest_def').trigger("create");
            },
            error: function (xhr, ajaxOptions, thrownError) {
                $('#loadingDiv').popup("close");
              
                alert('כשלון בפניה לשרת - אנא פנה לצוות כלים אוטומטים');
            },
            complete: function() {
    
                $("#refreshDoneDiv").show(2500);
                $("#refreshDoneDiv").hide(1500);
            }

        });
         
      
       
    });
   
  
     $('#resetDesc').unbind('click').click(function () {
        var req = sessionStorage.bugIDClicked;
        var res = "כן";
        $('#loadingDiv').popup("open");
        $.ajax({
            type: "POST",
            url: "forumProd.asmx/resetDesc",
            data: "{requestID:'" + req + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                if (msg.d == "\"Fail\"") {
                    alert('טיפול בבקשה לא הצליח - נא לפנות לצוות כלים אוטומטים');
                }
                if (msg.d == "\"Success\"" || msg.d == "\"Warning\"") {
                    alert('בוצע איפוס לבקשה מספר ' + sessionStorage.bugIDClicked);
                   
                    sessionStorage.resetDone = "1";
                    $.mobile.changePage("#page1", {
                        transition: "slideup",
                    });
                }
               $('#loadingDiv').popup("close");
            },
            error: function (xhr, ajaxOptions, thrownError) {
                $('#loadingDiv').popup("close");
                alert('כשלון בפניה לשרת - אנא פנה לצוות כלים אוטומטים');
            }

        });
    });
   
});



/// - Approved Today  - /////
$(document).on("pageshow","#approvedForAll", function () { 
     var geturl;
    $('#popupBasic').popup("open");
    $('#alltherequests').addClass('ui-disabled');
    $('#onlyMine').removeClass('ui-disabled');
    
    localStorage.amntofreqstodo = tableValues.length;
    $('#howmanyreqsTodo_3').text(tableValues.length);
    if(!window.navigator.onLine){
            alert ("אין חיבור לאינטרנט - לא תוכל לקבל נתונים");
            $('#popupBasic').popup("close");
    }
    else
    {
       
    
     $.ajax({
            type: "POST",
            url: "forumProd.asmx/getName",
            data: {},
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                
                if(JSON.parse(msg.d) == "notAllowed") {
               $('#popupBasic').popup("close");
                    alert('אין לך הרשאות  - אנא פנה לצוות כלים אוטומטים');
                     
                     $.mobile.changePage("#notAllowed", {
                        transition: "none"});
                }
                else if(JSON.parse(msg.d) == "ErrorFromServer") {
                    alert('כשלון בפניה באימות נתונים - נא בדוק התחברות');
                }
                else{
                   $('#hello_id').text(JSON.parse(msg.d));
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert('כשלון בפניה לשרת - אנא פנה לצוות כלים אוטומטים');
            }

        });

                    $.ajax({
                        type: "POST",
                        url: "forumProd.asmx/GetApprovedRequestsToday",
                        data: "{all:'true'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (msg) {
                            // Replace the div's content with the page method's return.
                            var summary = "";
                            var lstTweets = $('#approved_listview');
                            var tableValues = $.parseJSON(msg.d);
                            localStorage.tableValues = msg.d;
                            lstTweets.children().remove('li');
                            $('#ApprovednumberofRequests_lbl').text(tableValues.length);
                            for (var i = 0; i < tableValues.length; i++) {
                                var tmpsummary = tableValues[i].BG_SUMMARY;
                                if(tmpsummary.length > 20) {
                                        summary = tmpsummary.substring(0,19);
                                }
                                else {
                                        summary = tmpsummary;
                                }

                                lstTweets.append($('<li id=' + tableValues[i].BG_BUG_ID + '_Done><a href="#forumRequest_def" id=' + tableValues[i].BG_BUG_ID + '_lnk data-role="button" data-icon="arrow-r" data-transition="none"  data-iconpos="right" ><table style="width:100%"><tr><td>' + tableValues[i].BG_BUG_ID + ':</td><td><div>' + summary + '</div></td</tr></table></a></li>'));
                            }
                            if (tableValues.length == 0) {
                                lstTweets.children().remove('li');
                            }
                            lstTweets.listview('refresh');
                            $('#approved_listview').on('click', 'li', function () {
                                sessionStorage.bugIDClicked = $(this).attr('id');
                            });

                            $('#wichrequests_title').text('כל הבקשות שאושרו היום');
                        },
                        error: function (xhr, ajaxOptions, thrownError) {
                          
                        }

                    });
  
 $('#onlyMine').click(function() {
        $(this).addClass('ui-disabled');
        $('#alltherequests').removeClass('ui-disabled');
        
         $.ajax({
                        type: "POST",
                        url: "forumProd.asmx/GetApprovedRequestsToday",
                        data: "{all:'false'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (msg) {
                            // Replace the div's content with the page method's return.
                            var lstTweets = $('#approved_listview');
                            var tableValues = $.parseJSON(msg.d);
                            localStorage.tableValues = msg.d;
                            lstTweets.children().remove('li');
                            $('#ApprovednumberofRequests_lbl').text(tableValues.length);
                            for (var i = 0; i < tableValues.length; i++) {
                                    lstTweets.append($('<li id=' + tableValues[i].BG_BUG_ID + '_Done><a href="#forumRequest_def" id=' + tableValues[i].BG_BUG_ID + '_lnk data-role="button" data-icon="arrow-r" data-transition="none"  data-iconpos="right" >' + tableValues[i].BG_BUG_ID + '</a></li>'));
                            }
                            if (tableValues.length == 0) {
                                lstTweets.children().remove('li');
                            }
                            lstTweets.listview('refresh');
                            $('#approved_listview').on('click', 'li', function () {
                                sessionStorage.bugIDClicked = $(this).attr('id');
                            });

                             $('#wichrequests_title').text('כל הבקשות שלי שאושרו היום');
                        },
                        error: function (xhr, ajaxOptions, thrownError) {
                          
                        }

                    });
});

$('#alltherequests').click(function() {  
        $(this).addClass('ui-disabled');
        $('#onlyMine').removeClass('ui-disabled');
        
         $.ajax({
                        type: "POST",
                        url: "forumProd.asmx/GetApprovedRequestsToday",
                        data: "{all:'true'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (msg) {
                            // Replace the div's content with the page method's return.
                            var lstTweets = $('#approved_listview');
                            var tableValues = $.parseJSON(msg.d);
                            localStorage.tableValues = msg.d;
                            lstTweets.children().remove('li');
                            $('#ApprovednumberofRequests_lbl').text(tableValues.length);
                            for (var i = 0; i < tableValues.length; i++) {
                                    lstTweets.append($('<li id=' + tableValues[i].BG_BUG_ID + '_Done><a href="#forumRequest_def" id=' + tableValues[i].BG_BUG_ID + '_lnk data-role="button" data-icon="arrow-r" data-transition="none"  data-iconpos="right" >' + tableValues[i].BG_BUG_ID + '</a></li>'));
                            }
                            if (tableValues.length == 0) {
                                lstTweets.children().remove('li');
                            }
                            lstTweets.listview('refresh');
                            $('#approved_listview').on('click', 'li', function () {
                                sessionStorage.bugIDClicked = $(this).attr('id');
                            });
                            
                            $('#wichrequests_title').text('כל הבקשות שאושרו היום');
                        },
                        error: function (xhr, ajaxOptions, thrownError) {
                          
                        }

                    });
});        
           
}
    $('#popupBasic').popup("close");
});
/// -  Utility Functions - ///
function Isthere(slctr) {
    if ($(slctr).length > 0) {
        return true;
    }
    else {
        return false;
    }
}

function strip(html)
{
   var tmp = document.createElement("DIV");
   tmp.innerHTML = html;
   return tmp.textContent||tmp.innerText;
}








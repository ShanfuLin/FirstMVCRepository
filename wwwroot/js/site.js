let currPen = null;
const coral = "#ff7f50";
const cornflower = "#6495ed";

window.onload = function() {
    initListeners();
    restoreLast();
}

function initListeners() {
    // setup each node to listen to a mouse-click
    let nodes = document.getElementsByClassName("node");
    for (let i=0; i<nodes.length; i++) {
        nodes[i].addEventListener('click', onNodeClick);
    }

    // setup each color-pen to listen to a mouse-click
    let pens = document.getElementsByClassName("pen");
    for (let i=0; i<pens.length; i++) {
        pens[i].addEventListener('click', onPenClick);
    }
}

function onNodeClick(event) {
    if (currPen == null) {
        return;
    }

    if (currPen.id == "coral-pen") {
        event.target.style.background = coral; 
        SetNodeColor(event.target.id, coral);
    }
    else if (currPen.id == "cornflower-pen") {
        event.target.style.background = cornflower;
        SetNodeColor(event.target.id, cornflower);
    }
}

function onPenClick(event) {
    if (currPen != null) {
        // remove border from previously selected pen
        currPen.classList.remove("selected-pen");
    }

    // remember last selected pen
    currPen = event.target;

    // have a border around selected pen
    currPen.classList.add("selected-pen");
}

function restoreLast() {
    GetNodeColors();
}

/*
    'dict' contains 'nodeId' as key and a node's color as value.
*/
function onGetNodeColors(dict) {
    for (let nodeId in dict) {
        let node = document.getElementById(nodeId);
        if (node != null) {
            node.style.background = dict[nodeId];
        }
    }
}

function SetNodeColor(nodeId, color) {
    let ajax = new XMLHttpRequest();
    
    // need to recreate ajax object for each request (cannot re-use)
    ajax.open("POST", "/Home/SetNodeColor");
    ajax.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");

    ajax.onreadystatechange = function() {
        if (this.readyState == XMLHttpRequest.DONE) {
            if (this.status == 200) {
                // color string returned from server
                return this.responseText;
            }
        }
    }

    // key-value pairs
    ajax.send("nodeId=" + nodeId + "&color=" + color);
}

function GetNodeColors() {
    let ajax = new XMLHttpRequest();
    
    // need to recreate ajax object for each request (cannot re-use)
    ajax.open("GET", "/Home/GetNodeColors");

    ajax.onreadystatechange = function() {
        if (this.readyState == XMLHttpRequest.DONE) {
            if (this.status == 200) {
                // color string returned from server                                
                onGetNodeColors(JSON.parse(this.responseText));
            }
        }
    }

    ajax.send();
}

var source = new EventSource('/Home/Message');

source.onopen = function (e) {
    console.log('On Open: ', e);
};

source.onerror = function (e) {
    console.log('On Error: ', e);
};

source.onmessage = function (e) {
    onGetNodeColors(JSON.parse(e.data));
};    

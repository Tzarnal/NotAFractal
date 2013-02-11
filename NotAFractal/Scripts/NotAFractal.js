$(document).ready(function() {
    $(".toggleLink").click(toggle);
});

//Generates the content of a node
generate = function(nodeType, nodeSeed, element) {
    $.get('Node/GetNode/' + nodeType + '?Seed=' + nodeSeed, function(data) {
        //Add new nodes to page then mark this node as having generated its children
        $(element).append(data);
        $(element).data('generated', true);

        //Associate click events to new nodes, so they in turn can be expanded
        $(element).children(".nodeContainer").find('.toggleLink').click(toggle);

        //Finally open up the node for view
        $(element).data('open', true);        
        $(element).children(".nodeContainer").toggle('fast');
        $(element).children(".toggleLink").children(".treeViewToggleIndicator").text("-");
    });
};


toggle = function() {
    {
        var toggleIndicator = $(this).find(".treeViewToggleIndicator");
        var node = $(this).parent();

        //If this node has not been generated yet ( if its just a title and no content) that is, if its never been opened before
        //Generate its .nodeContainer through an ajax call

        if (node.data('generated') == false) {
            generate(node.data('type'), node.data('seed'), node);
        } else {
            //Togle the visible part of the node
            node.children(".nodeContainer").toggle('fast');

            //make the + a - or reverse
            if (node.data('open')) {
                toggleIndicator.text("+");
                node.data('open', false);
            } else {
                toggleIndicator.text("-");
                node.data('open', true);
            }
        }
    }
    ;
};
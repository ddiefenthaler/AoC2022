// TODO rewrite in C#

const fs = require('fs')

function compareLists(left, right) {
    if(typeof left === "number" && typeof right === "number") {
        return right - left;
    }
    if(typeof left === "number") {
        left = [left];
    }
    if(typeof right === "number") {
        right = [right];
    }

    for(var i=0; i < left.length; i++) {
        if(i >= right.length) return i - left.length;
        var diff_elem = compareLists(left[i],right[i]);
        if(diff_elem != 0) {
            return diff_elem;
        }
    }
    return right.length-left.length;
}

try {
    var lines = fs.readFileSync('input13_sample.txt', 'utf8');
    var pairs = lines.split(/\r?\n\r?\n/g).map(pair => pair.split(/\r?\n/g) );

    var sumIndices=0;
    for(var i=0 ; i < pairs.length; i++) {
        var left = JSON.parse(pairs[i][0]);
        var right = JSON.parse(pairs[i][1]);

        if(compareLists(left,right) >= 0) {
        //    console.log(compareLists(left,right));
           sumIndices += i+1;
        }
    }
    console.log(sumIndices);

    var messages = lines.split(/\r?\n/g).filter(m => m != "").map(m => JSON.parse(m));
    messages.push([[2]], [[6]]);
    console.log(messages);

    messages.sort(compareLists);
    messages.reverse();

    key1 = messages.findIndex(elem => elem.length == 1 && elem[0].length == 1 && elem[0][0] == 2)+1;
    key2 = messages.findIndex(elem => elem.length == 1 && elem[0].length == 1 && elem[0][0] == 6)+1;
    console.log(key1+" * "+key2+" = "+(key1*key2));
    console.log(JSON.stringify(messages[113]));
    console.log(JSON.stringify(messages[195]));
    console.log(114*196);
    // console.log(messages);
} catch (err) {
    console.error(err);
}

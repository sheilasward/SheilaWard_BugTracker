$(function () {
    'use strict'
    // Add the myProj listener
    $('li.myProj').on('click', function (e) {
        if ($(this).hasClass('active')) return
        //e.preventDefault()
        // Add "active" to this button
        $(this).addClass('active')
        $('li.allProj').removeClass('active')
    })
    // Add the allProj listener
    $('li.allProj').on('click', function (e) {
        if ($(this).hasClass('active')) return
        //e.preventDefault()
        $(this).addClass('active')
        $('li.myProj').removeClass('active')
    })
});
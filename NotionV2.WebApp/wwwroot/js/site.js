﻿// function BindCategoryNavItems() {
//     $("a[category-id]").click(function () {
//         let item = $(this).closest(".category-navbar-item");
//         let id = $(this).attr('category-id');
//
//         $("#searchCategory").val(id);
//
//         $.get("/Home/GetPostsHtml?category=" + id, function(data) {
//             $("#postsHtmlBody").html(data);
//
//             $(".category-navbar-item").removeClass("active");
//             item.addClass("active");
//
//             let articleNavs = $(".article-navbar-item a[post-id]");
//             if (articleNavs.length)
//             {
//                 articleNavs.first().click();
//             }
//             else
//             {
//                 $.get("/Home/GetPostHtml?post=-1", function(data) {
//                     $("#postHtmlBody").html(data);
//                 });
//             }
//         });
//     });
// }
//
// function BindPostNavItems() {
//     $("a[post-id]").click(function () {
//         let item = $(this).closest(".article-navbar-item");
//         let id = $(this).attr('post-id');
//
//         $.get("/Home/GetPostHtml?post=" + id, function(data) {
//             $("#postHtmlBody").html(data);
//
//             $(".article-navbar-item").removeClass("active");
//             item.addClass("active");
//         });
//     });
// }
//
// function BindCategoryControlButtons() {
//     $("#categoryCreateButton").click(function () {
//         $("#categoryEditDialog input[name='categoryId']").attr("value", "-1");
//         $("#categoryEditDialog input[name='categoryTitle']").attr("value", "");
//         $("#categoryEditDialog").modal();
//     });
//     $("#categoryDeleteButton").click(function () {
//         let item = $(".category-navbar-item.active");
//         let id = $(item).attr('category-id');
//
//         $("#categoryDeleteDialog a").attr("href", "/Home/DeleteCategory?categoryId=" + id);
//         $("#categoryDeleteDialog").modal();
//     });
//     $("#categoryEditButton").click(function () {
//         let item = $(".category-navbar-item.active");
//         let id = $(item).attr('category-id');
//         let text = $(item).text();
//
//         $("#categoryEditDialog input[name='categoryId']").attr("value", id);
//         $("#categoryEditDialog input[name='categoryTitle']").attr("value", text);
//         $("#categoryEditDialog").modal();
//     });
// }

function BindPostsControlButtons() {
    $("#postCreateButton").click(function () {

        $("#postEditDialog input[name='postId']").attr("value", "-1");
        $("#postEditDialog input[name='postTitle']").attr("value", "");
        $("#postEditDialog textarea[name='postText']").val("");

        $("#postEditDialog").show();
    });
    // $("#postDeleteButton").click(function () {
    //     let selectedPostItem = $(".article-navbar-item.active .stretched-link");
    //     let selectedPostId = $(selectedPostItem).attr('post-id');
    //
    //     $("#postDeleteDialog a").attr("href", "/Home/DeletePost?postId=" + selectedPostId);
    //     $("#postDeleteDialog").modal();
    // });
    // $("#postEditButton").click(function () {
    //     let selectedCategoryItem = $(".category-navbar-item.active");
    //     let selectedCategoryId = $(selectedCategoryItem).attr('category-id');
    //     let selectedPostItem = $(".article-navbar-item.active .stretched-link");
    //     let selectedPostId = $(selectedPostItem).attr('post-id');
    //     let selectedPostTitle = $(selectedPostItem).text();
    //     let selectedPostContents = $(".article-navbar-item.active .card-text").text();
    //
    //     $("#postEditDialog input[name='postId']").attr("value", selectedPostId);
    //     $("#postEditDialog input[name='categoryId']").attr("value", selectedCategoryId);
    //     $("#postEditDialog input[name='postTitle']").attr("value", selectedPostTitle);
    //     $("#postEditDialog textarea[name='postText']").val(selectedPostContents);
    //
    //     $("#postEditDialog").modal();
    // });
}
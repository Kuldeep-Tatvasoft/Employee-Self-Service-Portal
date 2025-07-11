// $(document).ready(function () {
//     function populateSubCategories(categoryId) {
//         const subCategoriesSelect = $('#SubCategories');
//         const subCategoryDropdownMenu = $('#subCategoryDropdownMenu');
//         const subCategoryCheckboxDropdown = $('#subCategoryCheckboxDropdown');

//         subCategoriesSelect.empty();
//         subCategoryDropdownMenu.empty();

//         if (categoryId == 3) {
//             // Show the checkbox dropdown and hide the standard dropdown
//             subCategoriesSelect.addClass('d-none');
//             subCategoryCheckboxDropdown.removeClass('d-none');

//             // Fetch subcategories and populate the dropdown with checkboxes
//             $.getJSON('@Url.Action("GetSubCategories", "HelpDesk")', { categoryId: categoryId }, function (subCategories) {
//                 $.each(subCategories, function (index, subCategory) {
//                     const checkboxItem = `
//                         <li>
//                             <div class="form-check">
//                                 <input class="form-check-input subCategoryCheckbox" type="checkbox" value="${subCategory.value}" id="subCategory_${subCategory.value}">
//                                 <label class="form-check-label" for="subCategory_${subCategory.value}">
//                                     ${subCategory.text}
//                                 </label>
//                             </div>
//                         </li>
//                     `;
//                     subCategoryDropdownMenu.append(checkboxItem);
//                 });

//                 // Update selected subcategories when checkboxes are clicked
//                 $('.subCategoryCheckbox').change(function () {
//                     updateSelectedSubCategories();
//                 });
//             });
//         } else {
//             // Show the standard dropdown and hide the checkbox dropdown
//             subCategoriesSelect.removeClass('d-none');
//             subCategoryCheckboxDropdown.addClass('d-none');

//             // Fetch subcategories and populate the standard dropdown
//             $.getJSON('@Url.Action("GetSubCategories", "HelpDesk")', { categoryId: categoryId }, function (subCategories) {
//                 subCategoriesSelect.append('<option value="">Select Sub Category</option>');
//                 $.each(subCategories, function (index, subCategory) {
//                     subCategoriesSelect.append($('<option/>', {
//                         value: subCategory.value,
//                         text: subCategory.text
//                     }));
//                 });
//             });
//         }
//     }

//     function updateSelectedSubCategories() {
//         const selectedSubCategories = [];
//         const selectedSubCategoryIds = [];

//         $('.subCategoryCheckbox:checked').each(function () {
//             selectedSubCategories.push($(this).next('label').text());
//             selectedSubCategoryIds.push($(this).val());
//         });

//         $('#selectedSubCategories').text(selectedSubCategories.length > 0 ? selectedSubCategories.join(', ') : 'Select Sub Categories');
//         $('#selectedSubCategoryIds').val(selectedSubCategoryIds.join(','));
//     }

//     // Handle category change
//     $('#Categories').change(function () {
//         const categoryId = $(this).val();
//         populateSubCategories(categoryId);
//     });

//     // Submit form via AJAX
//     $('#customDateApplyModel').click(function (e) {
//         e.preventDefault();

//         const formData = {
//             HelpDeskRequestId: $('#HelpDeskRequestId').val(),
//             EmployeeId: $('#EmployeeId').val(),
//             GroupId: $('#Groups').val(),
//             CategoryId: $('#Categories').val(),
//             SubCategoryIds: $('#selectedSubCategoryIds').val().split(','),
//             Priority: $('#Priority').val(),
//             ServiceDetails: $('#ServiceDetails').val(),
//             RequestedDate: $('#requestedDate').val()
//         };

//         $.ajax({
//             url: '/HelpDesk/AddEditHelpDeskRequest',
//             type: 'POST',
//             data: formData,
//             success: function (response) {
//                 if (response.success) {
//                     alert('Request submitted successfully!');
//                 } else {
//                     alert('Error submitting request.');
//                 }
//             },
//             error: function () {
//                 alert('An error occurred while submitting the request.');
//             }
//         });
//     });
// });

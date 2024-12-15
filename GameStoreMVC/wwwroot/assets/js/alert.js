document.querySelectorAll('.remove-btn').forEach(button => {
    button.addEventListener('click', function (e) {
        e.preventDefault();
        const itemId = this.getAttribute('data-id');

        Swal.fire({
            title: 'Are you sure?',
            text: "You won't be able to revert this!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, delete it!'
        }).then((result) => {
            if (result.isConfirmed) {
                window.location.href = `/Cart/RemoveItem?Id=${itemId}`;
            }
        });
    });
});
let desde = 10;

function MeGusta(idPublicacion) {

    fetch(
        '/Publicacion/MeGusta?idPublicacion=' + idPublicacion,
        {
            method: 'POST'
        }
    )
    .then(respuesta => respuesta.json())
    .then(datos => {

        if (datos.error == false) {

            document.getElementById(
                'likes-' + idPublicacion
            ).innerText = datos.cantidad;

            if (datos.tieneMeGusta == true) {

                document.getElementById(
                    'boton-like-' + idPublicacion
                ).innerText = 'Ya no me gusta';

            }
            else {

                document.getElementById(
                    'boton-like-' + idPublicacion
                ).innerText = 'Me gusta';

            }
        }

    });
}

function Comentar(idPublicacion) {

    let input =
        document.getElementById(
            'comentario-' + idPublicacion
        );

    let texto = input.value;

    fetch(
        '/Publicacion/Comentar?idPublicacion=' +
        idPublicacion +
        '&texto=' +
        encodeURIComponent(texto),
        {
            method: 'POST'
        }
    )
    .then(respuesta => respuesta.json())
    .then(datos => {

        if (datos.error == false) {

            let comentario =
                document.createElement('div');

            comentario.className = 'comentario';

            comentario.innerHTML =
                '<strong>' +
                datos.nombreUsuario +
                '</strong> ' +
                '<span>' +
                datos.texto +
                '</span> ' +
                '<small>' +
                datos.fecha +
                '</small>';

            document.getElementById(
                'comentarios-' + idPublicacion
            ).appendChild(comentario);

            input.value = '';
        }

    });
}

function VerMas() {

    fetch(
        '/Publicacion/ObtenerMas?desde=' + desde
    )
    .then(respuesta => respuesta.json())
    .then(publicaciones => {

        for (let i = 0; i < publicaciones.length; i++) {

            let publicacion = publicaciones[i];

            let article =
                document.createElement('article');

            article.className = 'publicacion';

            article.innerHTML =
                '<h2>' +
                publicacion.titulo +
                '</h2>' +

                '<img src="/img/' +
                publicacion.imagen +
                '">' +

                '<p>' +
                publicacion.descripcion +
                '</p>' +

                '<p>' +
                publicacion.fechaPublicacion +
                '</p>' +

                '<button type="button" ' +
                'id="boton-like-' +
                publicacion.id +
                '" onclick="MeGusta(' +
                publicacion.id +
                ')">Me gusta</button>' +

                '<span id="likes-' +
                publicacion.id +
                '">0</span> Me gusta' +

                '<section id="comentarios-' +
                publicacion.id +
                '"></section>' +

                '<input type="text" ' +
                'id="comentario-' +
                publicacion.id +
                '" placeholder="Escribí un comentario">' +

                '<button type="button" onclick="Comentar(' +
                publicacion.id +
                ')">Comentar</button>';

            document.getElementById(
                'publicaciones'
            ).appendChild(article);
        }

        desde = desde + 10;

        if (publicaciones.length < 10) {

            document.getElementById(
                'verMas'
            ).style.display = 'none';

        }

    });
}
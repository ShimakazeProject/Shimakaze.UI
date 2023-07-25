mod interop;

use std::ffi::CStr;

use interop::GetData;
use wry::{
    application::{
        event::{Event, StartCause, WindowEvent},
        event_loop::{ControlFlow, EventLoop},
        window::WindowBuilder,
    },
    http::{
        header::{ACCESS_CONTROL_ALLOW_ORIGIN, CONTENT_TYPE},
        Response, StatusCode,
    },
    webview::WebViewBuilder,
};

const PROTOCOL: &str = "shimakaze-ui";

#[no_mangle]
pub extern "C" fn run(title: *const i8, get_data: GetData) {
    let event_loop = EventLoop::new();
    let window = WindowBuilder::new()
        .with_title(unsafe { CStr::from_ptr(title).to_str().unwrap() })
        .build(&event_loop)
        .unwrap();

    let _webview = WebViewBuilder::new(window)
        .unwrap()
        .with_custom_protocol(PROTOCOL.into(), move |request| {
            let path = request.uri().path();
            println!("Request Path: {}", path);

            let res = get_data(path.as_ptr(), path.len());

            if res.is_null() {
                Response::builder()
                    .status(StatusCode::NOT_FOUND)
                    .body(Vec::new().into())
                    .map_err(Into::into)
            } else {
                Response::builder()
                    .header(CONTENT_TYPE, res.get_content_type())
                    .header(ACCESS_CONTROL_ALLOW_ORIGIN, "*")
                    .body(res.get_data().into())
                    .map_err(Into::into)
            }
        })
        .with_devtools(true)
        .with_url(
            #[cfg(target_os = "windows")]
            format!("https://{}.ui/", PROTOCOL).as_str(),
            #[cfg(not(target_os = "windows"))]
            format!("{}://", PROTOCOL).as_str(),
        )
        .unwrap()
        .build()
        .unwrap();

    event_loop.run(move |event, _, control_flow| {
        *control_flow = ControlFlow::Wait;

        match event {
            Event::NewEvents(StartCause::Init) => println!("Shimakaze.UI Framework started!"),
            Event::WindowEvent {
                event: WindowEvent::CloseRequested,
                ..
            } => *control_flow = ControlFlow::Exit,
            _ => (),
        }
    })
}

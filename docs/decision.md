# Decision Log

## 2026-04-27 - 빌드 플랫폼을 x86으로 유지

- 배경: 한글 COM 자동화 구성요소가 32비트 경로(`Program Files (x86)`)에 설치됨
- 결정: `MSBuild /p:Platform=x86` 기준으로 검증
- 이유: COM 상호운용 안정성 확보

## 2026-04-27 - COM 등록 검증 기준 채택

- 배경: TypeLib 등록만으로는 런타임 `Class not registered`가 해결되지 않음
- 결정: 실제 동작 기준을 `New-Object -ComObject HWPFrame.HwpObject` 성공으로 정의
- 이유: 레지스트리 경로 확인만으로는 32/64비트 뷰 차이로 오판 가능

## 2026-04-27 - old GUI DLL만으로 무설정 런타임 대체는 채택하지 않음

- 배경: old GUI 폴더에 `AxInterop/Interop.HWPCONTROLLib.dll`이 포함되어 있어 설정 제거 가능성 검토
- 결정: 인터롭 DLL 복사만으로 런타임 COM 의존 제거는 불가로 판단
- 이유:
  - 인터롭 DLL은 래퍼이며 COM 서버 본체가 아님
  - 현재 코드는 `HWPFrame.HwpObject` 경로를 사용하므로 등록 상태에 계속 의존

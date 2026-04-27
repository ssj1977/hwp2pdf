# Prompt History

## 2026-04-27 15:12:00 CLI 빌드 및 전포맷 변환 검증 요청

### User Request

hwp2pdf를 CLI로도 동작하게 하려고 작업중인데, 빌드해서 실행 잘되는지 확인해줘. test.hwp 를 테스트 문서로 활용해서 다른 포맷들로 전부 변환 시도해줘.

### Analysis & Actions

- 프로젝트 빌드 도구, CLI 엔트리, 지원 포맷 확인
- 빌드 실패 원인 파악(.NET 4.6.1 타기팅 팩 누락, COM 타입 라이브러리 미등록)
- COM 등록 경로 탐색 및 등록 검증 절차 수행
- `test.hwp`를 대상으로 전체 포맷 변환 실행

### Outputs

- 생성된 파일 목록
  - `out-cli-test-2` 하위 변환 결과물
- 수정된 파일 목록
  - 없음

## 2026-04-27 15:30:00 타기팅 팩 설치 완료 공유

### User Request

.NET Framework 4.6.1 설치했어.  2번은 어떻게 하는거야?

### Analysis & Actions

- COM 등록 상태를 레지스트리/파일 기준으로 재확인
- `HwpObject.tlb` 및 `HwpAutomation.dll` 등록 가능성 점검
- 관리자 권한이 필요한 등록 명령을 안내

### Outputs

- 생성된 파일 목록
  - 없음
- 수정된 파일 목록
  - 없음

## 2026-04-27 15:33:00 COM 등록 결과 공유

### User Request

PS C:\WINDOWS\system32> $bin = "C:\Program Files (x86)\Hnc\Office 2024\HOffice130\Bin"
PS C:\WINDOWS\system32> & "$env:WINDIR\SysWOW64\regsvr32.exe" "$bin\HwpAutomation.dll"
PS C:\WINDOWS\system32> & "$bin\Hwp.exe" /RegServer
PS C:\WINDOWS\system32> Test-Path "Registry::HKEY_CLASSES_ROOT\CLSID\{2291CF00-64A1-4877-A9B4-68CFE89612D6}"
False
PS C:\WINDOWS\system32> Test-Path "Registry::HKEY_CLASSES_ROOT\TypeLib\{7D2B6F3C-1D95-4E0C-BF5A-5EE564186FBC}"
True

### Analysis & Actions

- 32비트 COM 등록 뷰(`WOW6432Node`) 기준으로 확인하도록 가이드
- COM 객체 직접 생성 테스트로 최종 동작 판별

### Outputs

- 생성된 파일 목록
  - 없음
- 수정된 파일 목록
  - 없음

## 2026-04-27 15:34:00 32비트 등록 및 객체 생성 성공 공유

### User Request

 Test-Path "Registry::HKEY_CLASSES_ROOT\WOW6432Node\CLSID\{2291CF00-64A1-4877-A9B4-68CFE89612D6}"

True

### Analysis & Actions

- COM 등록 상태가 32비트 뷰에 존재함을 확인
- 즉시 전 포맷 변환 재검증 진행

### Outputs

- 생성된 파일 목록
  - 없음
- 수정된 파일 목록
  - 없음

## 2026-04-27 15:34:30 COM 개체 생성 성공 공유

### User Request

PS C:\Users\KU> try {
>>     $hwp = New-Object -ComObject HWPFrame.HwpObject
>>     Write-Host "성공: HwpObject가 정상적으로 생성되었습니다." -ForegroundColor Green
>>     $hwp.Quit()
>> } catch {
>>     Write-Host "실패: 개체를 생성할 수 없습니다. ($($_.Exception.Message))" -ForegroundColor Red
>> }
성공: HwpObject가 정상적으로 생성되었습니다.

### Analysis & Actions

- COM 자동화 가능 상태 확인
- 전체 포맷 변환 테스트 재실행

### Outputs

- 생성된 파일 목록
  - `out-cli-test-2\PDF\test.pdf`
  - `out-cli-test-2\HWPX\test.hwpx`
  - `out-cli-test-2\HWPML2X\test.hml`
  - `out-cli-test-2\HTMLplus\test.html`
  - `out-cli-test-2\OOXML\test.docx`
  - `out-cli-test-2\UNICODE\test.txt`
  - `out-cli-test-2\RTF\test.rtf`
- 수정된 파일 목록
  - 없음

## 2026-04-27 15:36:00 문서화 요청

### User Request

중간에 발생했던 문제나 해결방법, 설정하는 과정도 문서로 잘 정리해줘.
테스트문서를 모든 포맷으로 변경하는 명령어는 뭐야? 이것도 문서에 작성해줘

### Analysis & Actions

- README와 docs 체계 신규 작성
- 트러블슈팅, 설정 절차, 검증 결과, 일괄 변환 명령 문서화

### Outputs

- 생성된 파일 목록
  - `docs/PRD.md`
  - `docs/security_policy.md`
  - `docs/architecture.md`
  - `docs/implementation.md`
  - `docs/plan.md`
  - `docs/todo.md`
  - `docs/progress.md`
  - `docs/decision.md`
  - `docs/findings.md`
  - `docs/prompt_history.md`
- 수정된 파일 목록
  - `README.md`

## 2026-04-27 16:05:00 old GUI와 현재 CLI 차이 질의

### User Request

근데 예전 GUI 버전은 이런 설정 없이도 동작했는데, 무슨 차이야? C:\Users\KU\puhaha\hwp2pdf\hwp2pdf-old-gui 안의 빌드된 버전이 있어. dll 파일들 때문이야? 그럼 이런 dll을 활용해서 설정 없이도 동작하게 할 수 없어?

### Analysis & Actions

- `hwp2pdf-old-gui` 폴더 구성 파일(AxInterop/Interop DLL, exe) 확인
- IL 덤프 분석으로 old GUI의 COM 경로 확인(`AxHwpCtrl`, `HWPCONTROLLib`)
- 현재 코드의 COM 경로(`HwpObjectLib`, `HWPFrame.HwpObject`)와 비교
- DLL 역할(래퍼 vs COM 서버 본체) 구분 및 무설정 가능 범위 판단

### Outputs

- 생성된 파일 목록
  - 없음
- 수정된 파일 목록
  - 없음

## 2026-04-27 16:10:00 발견 내용 상세 문서화 요청

### User Request

발견한 내용은 문서로 자세히 정리해줘

### Analysis & Actions

- README 최근 변경 내역 갱신
- `docs/findings.md`에 old GUI vs 현재 CLI 차이, DLL 역할, 결론 상세 추가
- `docs/implementation.md`에 DLL 재사용 가능/불가 범위와 운영 가이드 추가
- `docs/architecture.md`, `docs/decision.md`, `docs/progress.md` 동기화

### Outputs

- 생성된 파일 목록
  - 없음
- 수정된 파일 목록
  - `README.md`
  - `docs/architecture.md`
  - `docs/implementation.md`
  - `docs/findings.md`
  - `docs/decision.md`
  - `docs/progress.md`
  - `docs/prompt_history.md`
